[<RequireQualifiedAccess>]
module Feliz.React.Msal.Requests

open System
open System.Collections.Generic
open Fable.Core

open Account

type SilentRequest = {
    scopes: string [] option
    redirectUri: string option;
    extraQueryParameters: Dictionary<string,string> option;
    tokenQueryParameters: Dictionary<string,string> option;
    authority: string option;
    account: AccountInfo option;
    correlationId: string option;
    forceRefresh: bool;
} with static member Default = {
        scopes= None
        redirectUri= None;
        extraQueryParameters= None;
        tokenQueryParameters= None;
        authority= None;
        account= None;
        correlationId= None;
        forceRefresh= false;
    }

type [<StringEnum>] [<RequireQualifiedAccess>] PopUpRequestPrompt =
    | [<CompiledName("login")>]Login
    | [<CompiledName("none")>]``None``
    | [<CompiledName("consent")>]Consent
    | [<CompiledName("select_account")>]SelectAccount
    | [<CompiledName("create")>]Create

type PopupPosition = {
    top: int;
    left: int;
}

type PopupSize = {
    height: int;
    width: int;
};

type PopupWindowAttributes = {
    popupSize: PopupSize option
    popupPosition: PopupPosition option
}

type  PopupRequest = {
    scopes: string[] option
    authority:string option
    correlationId:Guid option
    redirectUri:string option
    extraScopesToConsent: string[] option
    state:string option
    prompt: PopUpRequestPrompt
    loginHint:string option
    sid:string option
    domainHint:string option
    extraQueryParameters: Dictionary<string,string> option;
    tokenQueryParameters: Dictionary<string,string> option;
    claims:string option
    nonce:string option
    popupWindowAttributes:PopupWindowAttributes option
}
    with static member Default = {
            scopes= Some [||]
            authority=Some ""
            correlationId=Some Guid.Empty
            redirectUri=Some ""
            extraScopesToConsent= Some [||]
            state=Some ""
            prompt= PopUpRequestPrompt.Login
            loginHint=Some ""
            sid=Some ""
            domainHint=Some ""
            extraQueryParameters= None ;
            tokenQueryParameters= None;
            claims=Some ""
            nonce=Some ""
            popupWindowAttributes= None
        }

type RedirectRequest = {
    authority:string option
    account: AccountInfo option
    redirectUri: string option
    postLogoutRedirectUri: string option
}
