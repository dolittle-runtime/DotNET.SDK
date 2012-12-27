﻿using System;
using System.Dynamic;

namespace Bifrost.Commands
{
#pragma warning disable 1591 // Xml Comments
    public partial class DynamicCommand
    {
        dynamic _parameters;

        public bool IsBusy { get; set; }
        public bool IsProcessing { get; set; }
        public string Name { get; set; }
        public dynamic Parameters
        {
            get
            {
                if (_parameters == null)
                    _parameters = new ExpandoObject();
                return _parameters;
            }
        }

        public ICommandCoordinator CommandCoordinator { get; set; }

        public bool CanExecute(object parameter)
        {
            return true;
        }
        public event EventHandler CanExecuteChanged = (s, e) => { };

        public void Execute(object parameter)
        {
        }

        public event CommandResultsReceived CommandResultsReceived = (c, r) => { };

        public void OnCommandResultsReceived(Guid commandContextId, CommandResult result)
        {
            throw new NotImplementedException();
        }


        public event EventsProcessed EventsProcessed = (c) => { };

        public void OnEventsProcessed(Guid commandContextId)
        {
            throw new NotImplementedException();
        }
    }
#pragma warning restore 1591 // Xml Comments
}
