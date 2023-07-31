﻿namespace Telegram.Framework.Attributes.ValidateInputDataAttributes.UpdateDataNotNull
{
    public class RequireMessagePhotoAttribute : RequiredDataAttribute
    {
        public RequireMessagePhotoAttribute() : base(updateContext => updateContext.Message?.Photo)
        {
            ErrorMessage = "The photo of the message is required";
        }
    }
}