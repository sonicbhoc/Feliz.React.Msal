[<AutoOpen>]
module Feliz.React.Msal.Config

type Auth = {
    clientId:string
    authority:string
    knownAuthorities:string[]
    redirectUri:string
    postLogoutRedirectUri:string
}
type Cache = {
    cacheLocation:string;
    storeAuthStateInCookie:bool
}

type MsalConfig ={
    auth:Auth;
    cache:Cache}
