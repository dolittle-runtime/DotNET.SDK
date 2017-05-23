﻿using System;
using Bifrost.Commands;

namespace Bifrost.Specs.Security.Fakes
{
    public class SimpleCommand : ICommand
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string SomeString { get; set; }

        public int SomeInt { get; set; }
    }
}
