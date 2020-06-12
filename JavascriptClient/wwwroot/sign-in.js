var userManager = new Oidc.UserManager({ userStore: new Oidc.WebStorageStateStore({ store: window.localStorage })});

userManager.signinCallback().then(res => {
    console.log(res);
    window.location.href = '/home/index';
});