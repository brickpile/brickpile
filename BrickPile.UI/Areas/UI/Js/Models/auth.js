var RegisterModel = Backbone.Model.extend({
    urlRoot: '/api/auth',
    defaults: {
        username: null,
        password: null,
        confirmPassword: null
    }
});