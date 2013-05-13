var ContentEditView = Backbone.View.extend({
    events: {
        'click input[type=submit]': 'save'
    },
    //template: _.template($('#edit-template-article').html()),
    initialize: function () {
        try {
            var type = this.model.get('$type');
            var index = type.indexOf(',');
            this.template = _.template($('#edit-template-' + type.substring(0, index).replace(/\./g, '-').toLowerCase()).html());
        } catch (e) {
            //console.log('Unable to set editor template for model ' + e);
        }
    },
    render: function () {
        var html = this.template(this.model.toJSON());
        $(this.el).html(html);
        return this;
    },
    save: function (e) {
        e.preventDefault();
        var formData = this.$el.find('form').toObject();
        this.model.set(formData);
        this.model.save(formData);
    }
});
