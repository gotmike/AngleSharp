﻿namespace AngleSharp.DOM.Css
{
    using AngleSharp.Css;
    using AngleSharp.Extensions;
    using System;
    using System.Collections.Generic;
    
    /// <summary>
    /// Information can be found on MDN:
    /// https://developer.mozilla.org/en-US/docs/Web/CSS/content
    /// </summary>
    sealed class CSSContentProperty : CSSProperty, ICssContentProperty
    {
        #region Fields

        static readonly IValueConverter<ContentMode[]> Converter;
        static readonly ContentMode[] Normal;
        static readonly Dictionary<String, ContentMode> ContentModes;

        IEnumerable<ContentMode> _mode;

        #endregion

        #region ctor

        static CSSContentProperty()
        {
            ContentModes = new Dictionary<String, ContentMode>(StringComparer.OrdinalIgnoreCase);
            ContentModes.Add(Keywords.OpenQuote, new OpenQuoteContentMode());
            ContentModes.Add(Keywords.NoOpenQuote, new NoOpenQuoteContentMode());
            ContentModes.Add(Keywords.CloseQuote, new CloseQuoteContentMode());
            ContentModes.Add(Keywords.NoCloseQuote, new NoCloseQuoteContentMode());

            Normal = new[] { new NormalContentMode() };

            Converter = 
                TakeOne(Keywords.Normal, Normal).Or(
                TakeOne(Keywords.None, new ContentMode[0])).Or(
                TakeMany(
                    From(ContentModes).Or(
                    WithUrl().To(url => (ContentMode)new UrlContentMode(new Url(url)))).Or(
                    WithString().To(str => (ContentMode)new TextContentMode(str))).Or(
                    WithAttr().To(attr => (ContentMode)new AttributeContentMode(attr))).Or(
                    WithCounter().To(counter => (ContentMode)new CounterContentMode(counter)))
                ));
        }

        internal CSSContentProperty(CSSStyleDeclaration rule)
            : base(PropertyNames.Content, rule)
        {
            Reset();
        }

        #endregion

        #region Methods

        internal override void Reset()
        {
            _mode = Normal;
        }

        void SetMode(ContentMode[] mode)
        {
            _mode = mode;
        }

        /// <summary>
        /// Determines if the given value represents a valid state of this property.
        /// </summary>
        /// <param name="value">The state that should be used.</param>
        /// <returns>True if the state is valid, otherwise false.</returns>
        protected override Boolean IsValid(CSSValue value)
        {
            return Converter.TryConvert(value, SetMode);
        }

        #endregion

        #region Modes

        abstract class ContentMode
        {
            //TODO Add members that make sense
        }

        /// <summary>
        /// Computes to none for the :before and :after pseudo-elements.
        /// </summary>
        sealed class NormalContentMode : ContentMode
        {
        }

        /// <summary>
        /// The value is replaced by the open quote string from the quotes property.
        /// </summary>
        sealed class OpenQuoteContentMode : ContentMode
        {
        }

        /// <summary>
        /// The value is replaced by the close string from the quotes property.
        /// </summary>
        sealed class CloseQuoteContentMode : ContentMode
        {
        }

        /// <summary>
        /// Introduces no content, but increments the level of nesting for quotes.
        /// </summary>
        sealed class NoOpenQuoteContentMode : ContentMode
        {
        }

        /// <summary>
        /// Introduces no content, but decrements the level of nesting for quotes.
        /// </summary>
        sealed class NoCloseQuoteContentMode : ContentMode
        {
        }

        /// <summary>
        /// Text content.
        /// </summary>
        sealed class TextContentMode : ContentMode
        {
            readonly String _text;

            public TextContentMode(String text)
            {
                _text = text;
            }
        }

        /// <summary>
        /// The generated text is the value of all counters with
        /// the given name in scope at this pseudo-element, from
        /// outermost to innermost separated by the specified string.
        /// </summary>
        sealed class CounterContentMode : ContentMode
        {
            readonly Counter _counter;

            public CounterContentMode(Counter counter)
            {
                _counter = counter;
            }
        }

        /// <summary>
        /// Returns the value of the element's attribute X as a string.
        /// If there is no attribute X, an empty string is returned.
        /// </summary>
        sealed class AttributeContentMode : ContentMode
        {
            readonly String _attribute;

            public AttributeContentMode(String attribute)
            {
                _attribute = attribute;
            }
        }

        /// <summary>
        /// The value is a URI that designates an external resource (such as
        /// an image). If the resource or image can't be displayed, it is
        /// either ignored or some placeholder shows up.
        /// </summary>
        sealed class UrlContentMode : ContentMode
        {
            readonly Url _url;

            public UrlContentMode(Url url)
            {
                _url = url;
            }
        }

        #endregion
    }
}
