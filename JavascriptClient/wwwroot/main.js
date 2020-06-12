var oidcConfig = {
    authority: "https://localhost:44382/",
    client_id: "client_id_js",
    redirect_uri: "https://localhost:44335/Home/SignIn",
    response_type: "id_token token",
    scope: "openid ApiOne"
};

var userManager = new Oidc.UserManager(oidcConfig);

var signIn = function () {
    userManager.signinRedirect();
};

userManager.getUser().then(user => {
    //console.log(user);

    if (user) {
        axios.defaults.headers.common["Authorization"] = "Bearer " + user.access_token;
    }
});

var callApiOne = function () {
    axios.get('https://localhost:44371/secret').then(res => {
        console.log(res);
    });
};