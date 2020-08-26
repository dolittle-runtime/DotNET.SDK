// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Machine.Specifications;

namespace Dolittle.Machine.Specifications.MongoDB
{
    [Subject(typeof(a_mongo_db_instance))]
    public class when_running_a_spec_against_mongo : a_mongo_db_instance
    {
        It should_have_created_an_instance_of_the_mongo_runner = () => runner.ShouldNotBeNull();
        It should_have_created_a_database = () => database.ShouldNotBeNull();
        It should_have_created_a_client = () => client.ShouldNotBeNull();
        It should_have_generated_a_unique_db_name = () => database_name.ShouldNotBeEmpty();
        It should_expose_the_connection_string = () => connection_string.ShouldNotBeEmpty();
    }
}