var RegisterModel = Backbone.Model.extend({
    urlRoot: '/api/auth',
    username: '',
    password:'',
    confirmPassword:''
});