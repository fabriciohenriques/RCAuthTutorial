var userManager = new Oidc.UserManager({
    userStore: new Oidc.WebStorageStateStore({ store: window.localStorage }),
    response_mode: "query",
});

userManager.signinCallback().then(res => {
    console.log(res);
    window.location.href = '/home/index';
});