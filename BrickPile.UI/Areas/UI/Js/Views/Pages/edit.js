var PageEditView = Backbone.View.extend({
    events: {
        'click input[type=submit]': 'save'
    },
    template: _.template($('#edit-template-page').html()),
    initialize: function () {
        //this.collection.bind("reset", this.render, this);
    },
    render: function () {
        var html = this.template(this.model.toJSON());
        $(this.el).html(html);
        return this;
    },
    save: function (e) {
        e.preventDefault();
        var self = this;
        var formData = $('#myForm').toObject();
        this.model.set(formData);
        this.model.save(formData);
    }
});