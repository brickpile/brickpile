define([
        'jquery',
        'underscore',
        'backbone',
        'shortcuts',
        'brickpile',
        'form2js',
        'form2jsToObject',
        'views/base'
    ],
    function ($, _, Backbone, Shortcuts, Brickpile, Form2js, Form2jsToObject, BaseView) {
        
        var PageEditView = BaseView.extend({
            
            events: {
                'click input[type=submit]' : 'save',
                'click input[type=radio]' : function(e) {
                    alert($(e.currentTarget).val());
                }
            },
            
            shortcuts: {
                'ctrl+s, ⌘+s': 'save'
            },
            
            initialize: function () {
                
                _.extend(this, new Backbone.Shortcuts);
                this.delegateShortcuts();
                
                //this.bindTo(this.model, 'change', this.render);
                
                try {
                    var type = this.model.get('$type');
                    var index = type.indexOf(',');
                    this.template = _.template($('#edit-template-' + type.substring(0, index).replace(/\./g, '-').toLowerCase()).html());
                } catch (err) {
                    var txt = "There was an error on this page.\n\n";
                    txt += "Error description: Unable to set editor template for model " + err.message + "\n\n";
                    txt += "Click OK to continue.\n\n";
                    alert(txt);
                }
                
                //_.bindAll(this, "render");
                //this.model.bind('change', this.render);
            },
            render: function() {

                try {
                    var html = this.template(this.model.toJSON());
                    $(this.el).html(html);
                } catch(err) {
                    var txt = "There was an error on this page.\n\n";
                    txt += "Error description: " + err.message + "\n\n";
                    txt += "Click OK to continue.\n\n";
                    alert(txt);
                }

                return this;
            },           
            save: function (e) {
                
                e.preventDefault();
                
                // trigger event before save
                Brickpile.app.trigger('page:saving');
                
                var formData = $('#myForm').toObject();
                this.model.save(formData);

            }
        });
        return PageEditView;
    });