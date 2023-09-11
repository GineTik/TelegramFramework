﻿using System.Reflection;
using Telegramper.Executors.QueryHandlers.ParametersParser.Results;

namespace Telegramper.Executors.QueryHandlers.ParametersParser
{
    public interface IParametersParser
    {
        public ParametersParseResult Parse(string args, ICollection<ParameterInfo> parametersInfos, string argsSeparator);
    }
}
