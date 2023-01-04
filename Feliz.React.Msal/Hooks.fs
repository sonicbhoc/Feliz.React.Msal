module Feliz.React.Msal.Hooks

open Fable.Core
open Fable.Core.JsInterop

open Account

type AccountIdentifier =
    { localAccountId: string option
      homeAccountId: string option
      username: string option }

///The useIsAuthenticated hook returns a boolean indicating whether or not an account is signed in.
/// It optionally accepts an accountIdentifier object you can provide if you need to know whether or not
/// a specific account is signed in.
let useIsAuthenticated (accountIdentifier: AccountIdentifier option) : bool =
    import "useIsAuthenticated" "@azure/msal-react"

type IMsalContext =
    abstract member instance: IPublicClientApplication with get, set
    abstract member inProgress: InteractionStatus
    abstract member accounts: AccountInfo []

///The useAccount hook accepts an accountIdentifier parameter and returns the AccountInfo object for
/// that account if it is signed in or null if it is not. You can read more about the AccountInfo object
/// returned in the @azure/msal-browser docs here.
/// https://github.com/AzureAD/microsoft-authentication-library-for-js/blob/dev/lib/msal-browser/docs/login-user.md#account-apis
let useMsal () : IMsalContext = import "useMsal " "@azure/msal-react"

[<StringEnum>]
[<RequireQualifiedAccess>]
type InteractionType =
    | Redirect
    | Popup
    | Silent

[<RequireQualifiedAccess>]
type AuthenticationRequest =
    | Redirect of Requests.RedirectRequest
    | Popup of Requests.PopupRequest
    | Silent of Requests.SilentRequest

///The useMsalAuthentication hook will initiate a login if a user is not already signed in, otherwise it will attempt to acquire a token.
let useMsalAuthentication
    (
        interactionType: InteractionType,
        authenticationRequest: AuthenticationRequest option,
        accountIdentifier: AccountIdentifier option
    ) =
    import "useMsalAuthentication " "@azure/msal-react"

///The useAccount hook accepts an accountIdentifier parameter and returns the AccountInfo object for
/// that account if it is signed in or null if it is not. You can read more about the AccountInfo object
/// returned in the @azure/msal-browser docs here.
/// https://github.com/AzureAD/microsoft-authentication-library-for-js/blob/dev/lib/msal-browser/docs/login-user.md#account-apis
let useAccount (identifier: AccountIdentifiers) : AccountInfo =
    import "useAccount " "@azure/msal-react"
