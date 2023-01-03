namespace Feliz.React.Msal

open System.Collections.Generic
open System
open Fable.Core
open Fable.Core.JS
open Fable.Core.JsInterop
open Feliz

[<AutoOpen>]
module Msal =

    open Account

    let msalProvider : obj = import "MsalProvider" "@azure/msal-react"
    let authenticatedTemplate : obj = import "AuthenticatedTemplate"   "@azure/msal-react"
    let unauthenticatedTemplate : obj = import "UnauthenticatedTemplate"   "@azure/msal-react"

    ///<summary>Used to request scopes when requesting token</summary>
    type TokenRequest ={
      account:AccountInfo
      scopes:string[]
      forceRefresh:bool
    }

    type CommonSilentFlowRequest = {
        account: AccountInfo
        forceRefresh: bool
        tokenQueryParameters: Dictionary<string,string>;
    }

    type AuthenticationResult = {
        authority: string;
        uniqueId: string;
        tenantId: string;
        scopes: string[];
        account: AccountInfo option;
        idToken: string;
        IIdTokenClaims: IIdTokenClaims;
        accessToken: string;
        fromCache: bool;
        expiresOn:DateTime option;
        tokenType: string;
        correlationId: string;
        extExpiresOn: DateTime;
        state: string;
        familyId: string;
        cloudGraphHostName: string;
        msGraphHost: string;
    }

    type AuthError = {
        errorCode: string;
        errorMessage: string;
        subError: string;
        correlationId: string;
    }

    type IPublicClientApplication =
        abstract member loginRedirect: request:Requests.RedirectRequest -> unit;
        abstract member loginPopup: request:Requests.PopupRequest -> Promise<Result<AuthenticationResult,AuthError>>
        abstract member logout: unit-> unit
        abstract member logoutRedirect: request:Requests.RedirectRequest -> Promise<unit>
        abstract member getAllAccounts: unit-> AccountInfo[]
        abstract member acquireTokenSilent: request:Requests.SilentRequest -> Promise<AuthenticationResult option>;
        abstract member getAccountByUsername:userName: string -> AccountInfo option
        abstract member setActiveAccount:account: AccountInfo option -> unit
        abstract member getActiveAccount:unit -> AccountInfo option

    [<Import("PublicClientApplication", from="@azure/msal-browser")>]
    type PublicClientApplication (config:MsalConfig) =
        interface IPublicClientApplication with
            member _.loginRedirect(request:Requests.RedirectRequest) = jsNative
            member _.loginPopup(request:Requests.PopupRequest) = jsNative
            member _.logout() = jsNative
            member _.logoutRedirect(request:Requests.RedirectRequest) = jsNative
            member _.getAllAccounts() : AccountInfo[] = jsNative
            member _.acquireTokenSilent(request:Requests.SilentRequest) = jsNative
            member _.getAccountByUsername(userName:string) = jsNative
            member _.setActiveAccount(account: AccountInfo option) = jsNative
            member _.getActiveAccount():AccountInfo option = jsNative

    type [<StringEnum>] [<RequireQualifiedAccess>] InteractionStatus =
        /// Initial status before interaction occurs
        | Startup
        /// Status set when all login calls occuring
        | Login
        /// Status set when logout call occuring
        | Logout
        /// Status set for acquireToken calls
        | AcquireToken
        /// Status set for ssoSilent calls
        | SsoSilent
        /// Status set when handleRedirect in progress
        | HandleRedirect
        /// Status set when interaction is complete
        | None

    /// <summary>All components underneath MsalProvider will have access to the PublicClientApplication instance via
    /// context as well as all hooks and components provided by @azure/msal-react.
    /// for more info see
    /// <seealso cref="https://github.com/AzureAD/microsoft-authentication-library-for-js/blob/dev/lib/msal-react/docs/getting-started.md"/>
    /// </summary>
    type MsalProvider =
      static member inline instance (pca: obj) = "instance" ==> pca
      static member inline children (children: seq<Fable.React.ReactElement>) = "children" ==> children
      static member inline create props = Interop.reactApi.createElement (msalProvider, createObj !!props)

    /// <summary>Used to show UI when user is authenticated
    /// <seealso cref="https://github.com/AzureAD/microsoft-authentication-library-for-js/blob/dev/lib/msal-react/docs/getting-started.md"/>
    /// </summary>
    type AuthenticatedTemplate =
      static member inline children (children: seq<Fable.React.ReactElement>) = "children" ==> children
      static member inline create props = Interop.reactApi.createElement (authenticatedTemplate, createObj !!props)

    /// <summary>Used to show UI when user is NOT authenticated
    /// <seealso cref="https://github.com/AzureAD/microsoft-authentication-library-for-js/blob/dev/lib/msal-react/docs/getting-started.md"/>
    /// </summary>
    type UnauthenticatedTemplate  =
      static member inline children (children: seq<Fable.React.ReactElement>) = "children" ==> children
      static member inline create props = Interop.reactApi.createElement (unauthenticatedTemplate, createObj !!props)

    ///Send request to server to reset user password.
    let forgotPasswordRequest(config:MsalConfig):Requests.RedirectRequest = {
        account = None
        authority=Some config.auth.authority
        postLogoutRedirectUri=Some config.auth.postLogoutRedirectUri
        redirectUri=Some config.auth.redirectUri
    }
    ///Send request to server to edit user profile.
    let editProfileRequest(config:MsalConfig):Requests.RedirectRequest = {
        account = None
        authority=Some config.auth.authority
        postLogoutRedirectUri=Some config.auth.postLogoutRedirectUri
        redirectUri=Some config.auth.redirectUri
    }

    let forgotPassword (error:string) =
      error.Contains("AADB2C90118")

    let createClient config = new PublicClientApplication(config:MsalConfig) :> IPublicClientApplication

    type User = {
        FirstName:string
        Lastname:string
        DisplayName:string
        Email:string
        AccountInfo:AccountInfo
    }
    with
        member this.IsAdmin =
            true
        static member Default = {
            FirstName=""
            Lastname=""
            DisplayName=""
            Email=""
            AccountInfo = AccountInfo.Default()
        }

    let CreateUser(account:AccountInfo option):User =
        match account with
        | Some account ->
            match account.IIdTokenClaims with
            | Some token ->
                {
                    FirstName= token.given_name
                    Lastname= token.family_name
                    Email = token.emails[0]
                    DisplayName = $"{token.given_name[0]}.{token.family_name}"
                    AccountInfo = account
                }
            | None -> User.Default
        | _ -> User.Default

    ///Use this to request token from auth server
    let tokenRequest account scopes :Requests.SilentRequest=
        {   Requests.SilentRequest.Default with
                account= Some account
                scopes= Some scopes
                forceRefresh=false
        }

    let redirectRequest msalConfig redirectUri :Requests.RedirectRequest= {
      account = None
      authority =Some msalConfig.auth.authority
      postLogoutRedirectUri=Some redirectUri
      redirectUri = Some msalConfig.auth.redirectUri
    }

    let popupRequest msalConfig =
        {
            Requests.PopupRequest.Default with
                authority =Some msalConfig.auth.authority
                redirectUri = Some msalConfig.auth.redirectUri
        }
