var createState = function () {
    return "StateValueStateValueStateValueStateValueStateValueStateValueStateValueStateValueStateValueStateValueStateValueStateValueStateValueStateValueStateValue";
};

var createNonce = function () {
    return "NonceValueNonceValueNonceValueNonceValueNonceValueNonceValueNonceValueNonceValueNonceValueNonceValueNonceValueNonceValueNonceValueNonceValueNonceValue";
};

var signIn = function () {
    var redirectUri = "https://localhost:44335/Home/SignIn";
    var responseType = "id_token token";
    var scope = "openid ApiOne";

    var authUrl = "/connect/authorize/callback" + "?client_id=client_id_js" + `&redirect_uri=${redirectUri}` + `&response_type=${responseType}` + `&scope=${scope}` + `&nonce=${createNonce()}` + `&state=${createState()}`;

    var returnUrl = encodeURIComponent(authUrl);
    window.location.href = "https://localhost:44382/Auth/Login?ReturnUrl=" + returnUrl;
}