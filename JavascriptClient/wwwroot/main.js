var oidcConfig = {
    authority: "https://localhost:44382/",
    client_id: "client_id_js",
    redirect_uri: "https://localhost:44335/Home/SignIn",
    response_type: "id_token token",
    scope: "openid ApiOne ApiTwo rc.scope",
    userStore: new Oidc.WebStorageStateStore({ store: window.localStorage }),
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

var refreshing = false;

axios.interceptors.response.use(response => response,
    error => {
        console.error(error.response);

        var axiosConfig = error.response.config;
        if (error.response.status === 401) {
            if (!refreshing) {
                refreshing = true;

                return userManager.signinSilent().then(user => {
                    console.log("res: ", user);
                    axios.defaults.headers.common["Authorization"] = "Bearer " + user.access_token;
                    axiosConfig.headers["Authorization"] = "Bearer " + user.access_token;
                    refreshing = false;
                    return axios(axiosConfig);
                });
            }
        }

        return Promise.reject(error);
    });