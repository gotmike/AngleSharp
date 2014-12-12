﻿namespace AngleSharp.DOM.Css
{
    using AngleSharp.Css;
    using AngleSharp.Extensions;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// More information available at:
    /// https://developer.mozilla.org/en-US/docs/CSS/animation
    /// </summary>
    sealed class CSSAnimationProperty : CSSShorthandProperty, ICssAnimationProperty
    {
        #region Fields

        internal static readonly IValueConverter<Tuple<Time, ITimingFunction, Time, Single, AnimationDirection, AnimationFillStyle, PlayState, Tuple<String>>[]> Converter = Converters.WithAny(
            CSSAnimationDurationProperty.SingleConverter.Option(CSSAnimationDurationProperty.Default),
            CSSAnimationTimingFunctionProperty.SingleConverter.Option(CSSAnimationTimingFunctionProperty.Default),
            CSSAnimationDelayProperty.SingleConverter.Option(CSSAnimationDelayProperty.Default),
            CSSAnimationIterationCountProperty.SingleConverter.Option(CSSAnimationIterationCountProperty.Default),
            CSSAnimationDirectionProperty.SingleConverter.Option(CSSAnimationDirectionProperty.Default),
            CSSAnimationFillModeProperty.SingleConverter.Option(CSSAnimationFillModeProperty.Default),
            CSSAnimationPlayStateProperty.SingleConverter.Option(CSSAnimationPlayStateProperty.Default),
            CSSAnimationNameProperty.SingleConverter.Option(CSSAnimationNameProperty.Default)).FromList();

        readonly CSSAnimationDelayProperty _delay;
        readonly CSSAnimationDirectionProperty _direction;
        readonly CSSAnimationDurationProperty _duration;
        readonly CSSAnimationFillModeProperty _fillMode;
        readonly CSSAnimationIterationCountProperty _iterationCount;
        readonly CSSAnimationNameProperty _name;
        readonly CSSAnimationTimingFunctionProperty _timingFunction;
        readonly CSSAnimationPlayStateProperty _playState;

        #endregion

        #region ctor

        internal CSSAnimationProperty(CSSStyleDeclaration rule)
            : base(PropertyNames.Animation, rule)
        {
            _delay = Get<CSSAnimationDelayProperty>();
            _direction = Get<CSSAnimationDirectionProperty>();
            _duration = Get<CSSAnimationDurationProperty>();
            _fillMode = Get<CSSAnimationFillModeProperty>();
            _iterationCount = Get<CSSAnimationIterationCountProperty>();
            _name = Get<CSSAnimationNameProperty>();
            _timingFunction = Get<CSSAnimationTimingFunctionProperty>();
            _playState = Get<CSSAnimationPlayStateProperty>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the durations for the animations.
        /// </summary>
        public IEnumerable<Time> Durations
        {
            get { return _duration.Durations; }
        }

        /// <summary>
        /// Gets the offsets for the animations.
        /// </summary>
        public IEnumerable<Time> Delays
        {
            get { return _delay.Delays; }
        }

        /// <summary>
        /// Gets the timing-functions for the animations.
        /// </summary>
        public IEnumerable<ITimingFunction> TimingFunctions
        {
            get { return _timingFunction.TimingFunctions; }
        }

        /// <summary>
        /// Gets the names of the animations.
        /// </summary>
        public IEnumerable<String> Names
        {
            get { return _name.Names; }
        }

        /// <summary>
        /// Gets the fill modes of the animations.
        /// </summary>
        public IEnumerable<AnimationFillStyle> FillModes
        {
            get { return _fillMode.FillModes; }
        }

        /// <summary>
        /// Gets the directions of the animations.
        /// </summary>
        public IEnumerable<AnimationDirection> Directions
        {
            get { return _direction.Directions; }
        }

        /// <summary>
        /// Gets the iteraction counts of the animations.
        /// </summary>
        public IEnumerable<Single> Iterations
        {
            get { return _iterationCount.Iterations; }
        }

        public IEnumerable<PlayState> States
        {
            get { return _playState.States; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Determines if the given value represents a valid state of this property.
        /// </summary>
        /// <param name="value">The state that should be used.</param>
        /// <returns>True if the state is valid, otherwise false.</returns>
        protected override Boolean IsValid(ICssValue value)
        {
            return Converter.TryConvert(value, t =>
            {
                _duration.SetDurations(t.Select(m => m.Item1));
                _timingFunction.SetTimingFunctions(t.Select(m => m.Item2));
                _delay.SetDelays(t.Select(m => m.Item3));
                _iterationCount.SetIterations(t.Select(m => m.Item4));
                _direction.SetDirections(t.Select(m => m.Item5));
                _fillMode.SetFillModes(t.Select(m => m.Item6));
                _playState.SetStates(t.Select(m => m.Item7));
                _name.SetNames(t.Select(m => m.Rest.Item1));
            });
        }

        internal override String SerializeValue(IEnumerable<CSSProperty> properties)
        {
            if (!properties.Contains(_name) || !properties.Contains(_duration))
                return String.Empty;

            var values = new List<String>();
            values.Add(_name.SerializeValue());
            values.Add(_duration.SerializeValue());

            if (_timingFunction.HasValue && properties.Contains(_timingFunction))
                values.Add(_timingFunction.SerializeValue());

            if (_delay.HasValue && properties.Contains(_delay))
                values.Add(_delay.SerializeValue());

            if (_iterationCount.HasValue && properties.Contains(_iterationCount))
                values.Add(_iterationCount.SerializeValue());

            if (_direction.HasValue && properties.Contains(_direction))
                values.Add(_direction.SerializeValue());

            if (_fillMode.HasValue && properties.Contains(_fillMode))
                values.Add(_fillMode.SerializeValue());

            if (_playState.HasValue && properties.Contains(_playState))
                values.Add(_playState.SerializeValue());
            
            return String.Join(" ", values);
        }

        #endregion
    }
}
