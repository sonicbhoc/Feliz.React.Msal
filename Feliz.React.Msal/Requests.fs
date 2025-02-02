﻿[<RequireQualifiedAccess>]
module Feliz.React.Msal.Requests

open System
open System.Collections.Generic
open Fable.Core

open Account

type SilentRequest =
    { scopes: string [] option
      redirectUri: string option
      extraQueryParameters: Dictionary<string, string> option
      tokenQueryParameters: Dictionary<string, string> option
      authority: string option
      account: AccountInfo option
      correlationId: string option
      forceRefresh: bool }
    static member Default =
        { scopes = None
          redirectUri = None
          extraQueryParameters = None
          tokenQueryParameters = None
          authority = None
          account = None
          correlationId = None
          forceRefresh = false }

[<StringEnum>]
[<RequireQualifiedAccess>]
type PopUpRequestPrompt =
    | [<CompiledName("login")>] Login
    | [<CompiledName("none")>] None
    | [<CompiledName("consent")>] Consent
    | [<CompiledName("select_account")>] SelectAccount
    | [<CompiledName("create")>] Create

type PopupPosition = { top: int; left: int }

type PopupSize = { height: int; width: int }

type PopupWindowAttributes =
    { popupSize: PopupSize option
      popupPosition: PopupPosition option }

type PopupRequest =
    { scopes: string [] option
      authority: string option
      correlationId: Guid option
      redirectUri: string option
      extraScopesToConsent: string [] option
      state: string option
      prompt: PopUpRequestPrompt
      loginHint: string option
      sid: string option
      domainHint: string option
      extraQueryParameters: Dictionary<string, string> option
      tokenQueryParameters: Dictionary<string, string> option
      claims: string option
      nonce: string option
      popupWindowAttributes: PopupWindowAttributes option }
    static member Default =
        { scopes = Some [||]
          authority = Some ""
          correlationId = Some Guid.Empty
          redirectUri = Some ""
          extraScopesToConsent = Some [||]
          state = Some ""
          prompt = PopUpRequestPrompt.Login
          loginHint = Some ""
          sid = Some ""
          domainHint = Some ""
          extraQueryParameters = None
          tokenQueryParameters = None
          claims = Some ""
          nonce = Some ""
          popupWindowAttributes = None }

type RedirectRequest =
    { authority: string option
      account: AccountInfo option
      redirectUri: string option
      postLogoutRedirectUri: string option }

///<summary>Used to request scopes when requesting token</summary>
type TokenRequest =
    { account: AccountInfo
      scopes: string []
      forceRefresh: bool }

type CommonSilentFlowRequest =
    { account: AccountInfo
      forceRefresh: bool
      tokenQueryParameters: Dictionary<string, string> }

///Send request to server to reset user password.
let forgotPasswordRequest (config: MsalConfig) : RedirectRequest =
    { account = None
      authority = Some config.auth.authority
      postLogoutRedirectUri = Some config.auth.postLogoutRedirectUri
      redirectUri = Some config.auth.redirectUri }

///Send request to server to edit user profile.
let editProfileRequest (config: MsalConfig) : RedirectRequest =
    { account = None
      authority = Some config.auth.authority
      postLogoutRedirectUri = Some config.auth.postLogoutRedirectUri
      redirectUri = Some config.auth.redirectUri }

///Use this to request token from auth server
let tokenRequest account scopes : SilentRequest =
    { SilentRequest.Default with
        account = Some account
        scopes = Some scopes
        forceRefresh = false }

let redirectRequest msalConfig redirectUri : RedirectRequest =
    { account = None
      authority = Some msalConfig.auth.authority
      postLogoutRedirectUri = Some redirectUri
      redirectUri = Some msalConfig.auth.redirectUri }

let popupRequest msalConfig =
    { PopupRequest.Default with
        authority = Some msalConfig.auth.authority
        redirectUri = Some msalConfig.auth.redirectUri }
