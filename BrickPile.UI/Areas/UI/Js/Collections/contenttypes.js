define([
        'jquery',
        'underscore',
        'backbone',
        'models/contenttype'
    ], function($, _, Backbone, ContentType) {
        var ContentTypeCollection = Backbone.Collection.extend({
            model: ContentType
        });
        return ContentTypeCollection;
    });