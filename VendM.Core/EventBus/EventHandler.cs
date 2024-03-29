﻿using System;

namespace VendM.Core.EventBus
{
    public class EventHandler<TEvent> : IEventHandler<TEvent> where TEvent : IEvent
    {
        #region Private Fields
        private readonly Action<TEvent> action;
        #endregion

        #region Ctor
        /// <summary>
        /// Initializes a new instance of <c>ActionDelegatedEventHandler{TEvent}</c> class.
        /// </summary>
        /// <param name="action">The <see cref="Action{T}"/> instance that delegates the event handling process.</param>

        public EventHandler(Action<TEvent> action)
        {
            this.action = action;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Returns a <see cref="Boolean"/> value which indicates whether the current
        /// <c>ActionDelegatedEventHandler{T}</c> equals to the given object.
        /// </summary>
        /// <param name="obj">The <see cref="Object"/> which is used to compare to
        /// the current <c>ActionDelegatedEventHandler{T}</c> instance.</param>
        /// <returns>If the given object equals to the current <c>ActionDelegatedEventHandler{T}</c>
        /// instance, returns true, otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
                return true;
            if (obj == null)
                return false;
            EventHandler<TEvent> other = obj as EventHandler<TEvent>;
            if (other == null)
                return false;
            return Delegate.Equals(this.action, other.action);
        }

        #endregion

        #region IHandler<TDomainEvent> Members
        /// <summary>
        /// Handles the specified message.
        /// </summary>
        /// <param name="message">The message to be handled.</param>
        public void Handle(TEvent message)
        {
            action(message);
        }

        #endregion
    }
}
