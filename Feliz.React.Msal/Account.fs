module Feliz.React.Msal.Account

type IIdTokenClaims =
    abstract member aud: string with get, set
    abstract member auth_time: string with get, set
    abstract member emails: string[] with get, set
    abstract member exp: int with get, set
    abstract member family_name: string with get, set
    abstract member given_name: string with get, set
    abstract member iat: int with get, set
    abstract member iss: string with get, set
    abstract member nbf: int with get, set
    abstract member nonce: string with get, set
    abstract member sub: string with get, set
    abstract member tfp: string with get, set
    abstract member ver: string with get, set

type AccountInfo = {
    homeAccountId: string;
    environment: string;
    tenantId: string;
    username: string;
    localAccountId: string;
    name: string;
    IIdTokenClaims: IIdTokenClaims option;
}
  with
    static member Default() = {
        homeAccountId=""
        environment=""
        tenantId=""
        username=""
        localAccountId=""
        name=""
        IIdTokenClaims=None
    }

type AccountIdentifiers =
  | [<CompiledName("localAccount")>]LocalAccount of string
  | [<CompiledName("homeAccount")>]HomeAccount of string
  | [<CompiledName("username")>]Username of string
