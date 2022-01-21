# Customer AppInsights (Telemetry)

## Introduction

This distribution is intended for installation into a dev box for review and testing. A deployable module is included, however, I strongly recommend you bring this into your dev workflow, get familiar with it, test it, etc. 

This module is design to emit traces and information around activities occurring in the application space of D365. It is designed to emit Infologs and exceptions as traces, form init/run times as Requests, and to wrap commerce RTS calls and batch jobs (both RunBaseBatch and SysOperation framework-based) to give context to messages and errors emitted during such executions.

To keep things as simple as possible, this module uses the _Operational Insights_ forms and tables to determine which Instrumentation Key to use for a given environment. _Operational Insights_ is currently behind a feature flag and must be enabled in Feature Management.

## Source installation

### Prerequisites

The class library that handles managing the actual AppInsights client requires the NuGet packages for AppInsights and its dependencies be obtained from the online NuGet repo. By default, it seems that VS on dev boxes is configured only to use AX's local package repo. Thus, you will need to add a repository entry to allow VS to fetch the dependencies for this library.

In VS2019, open NuGet Package Manager Settings (Tools | NuGet Package Manager | Package Manager Settings), navigate to Package Sources, and add an entry for nuget.org as follows:
Name: nuget.org
Source: https://api.nuget.org/v3/index.json

You will also need an Application Insights instance to be setup and ready to receive telemetry. Based on the design of the Operational Insights module, you may setup different app insights instances to segment Dev, Test, and Prod from one another. At minimum, you will need at least once instance to be able to receive telemetry.

### Installing

On a tier 1 dev box, clone the repository to a location on your dev box that makes sense to you. 

Copy the source\PackagesLocalDirectory\CustomerAppInsights folder to your K:\AosService\PackagesLocalDirectory folder. Open the solution (located in source\Project\CustomerAppInsights), and build the whole solution (NOTE: make sure you build with Synchronize Database on Build set to true or AOS will fail to start up. The project is distributed with this configuration already in place, but just pay attention to this as failing to sync the db when applying this code for the first time _will_ cause the AOS to fail to start. Should this happen, simply go back to VS and do a db sync, and you should be fine. When deploying to higher environments, db sync is automatically performed on your behalf, so there should never be an issue there).

## Configuration

As of this writing, I don't see a docs page yet for Operational Insights, so I'll provide some basic guidance here to get your dev box going. 

### Configuring a dev instance
1. Navigate to the _Operational insights_ configuration form (System Administration | Setup | Operational insights)
2. Find the Environment Id GUID for your dev box (you can find this in the LCS details page for your cloud-hosted dev box and tier 2+ environments)
3. On the _Environments_ tab, paste in the Environment Id from #2, and set (or leave) the Environment Mode to _Development_ (assuming this is a dev box).
4. On the _Application Insights registry_ tab, hit ALT+N if necessary to add a new record, and provide the Instrumentation Key from your app insights instance. Leave or set the Environment Mode to Development (again, assuming you're doing this for your dev box).
5. On the _Configure_ tab, enable the toggle for _Capture custom events_.

The telemetry client is implemented as a session static instance, so once you enable this in the module, it will gradually take effect as new sessions are started. To accelerate, you can restart the d365 instance so that it boots up with the configurations enabled. The telemetry data typically shows up in app insights within about 2-5 minutes.

## License and Copyright

Copyright 2022 Bryon Williams

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
