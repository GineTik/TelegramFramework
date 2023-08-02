﻿using Telegramper.Attributes.BaseAttributes;
using Telegramper.TelegramBotApplication.Context;

namespace Telegramper.Attributes.ValidateInputDataAttributes.UpdateDataNotNull
{
    public class RequireDataAttribute : ValidateInputDataAttribute
    {
        protected Func<UpdateContext, object?> TakeProperty;

        public RequireDataAttribute(Func<UpdateContext, object?> takeProperty)
        {
            if (takeProperty == null)
                throw new InvalidOperationException("Func TakeProperty is null");

            TakeProperty = takeProperty;
        }

        public override async Task<bool> ValidateAsync(UpdateContext updateContext, IServiceProvider provider)
        {
            return await Task.FromResult(TakeProperty.Invoke(updateContext) != null);
        }
    }
}