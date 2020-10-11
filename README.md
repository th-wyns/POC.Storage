# Description
Proof of concept for a custom storage implementation. The implementation uses a hybrid of SQL and NoSQL/Lucene storing mechanism. SQL is used for storgin original data and reporting and NoSQL/Lucene is used for search. Over the abstraction layers the solution has a MsSQL-Elasticsearch-Networkshare sample implementation. These can be replaced with other implementations like MySQL, PostgreSQL, SOLR, Azure Blob.

# Criteria
- Add 100 contents per second
- Update 5500 contents per second
- All metadata should be indexed and search with features matching dtSearch/Lucene capabilities
- Metadatas should be used in reporting applied with functions like *MIN, MAX, MEAN, CUSTODIAN...*

# Prerequisities
- Download and install [.NET Core SDK 3.1.201](https://dotnet.microsoft.com/download/dotnet-core/3.1)
- Download and install [Elasticsearch](https://www.elastic.co/downloads/elasticsearch)  
 Leave it on default port 9200 or update the appsettings file for the test projects
- Install MsSql (at least version 2016) on defult instance or update the appsettings file for the test projects if you're using a named instance

# Solution Hierarchy
The soulution has two major folders:

- **src**: contains the source code
- **tests**: cotains the unit tests and a test Importer application to test creation and update processes with sample load files

## Source Projects
- **POC.Storage.Core** : the implementation indepent code base which contains abstractions and interfaces
- **POC.Storage.Elasticsearch** : Elasticsearch based implementations
- **POC.Storage.MsSql** : MsSql based implementations
- **POC.Storage.NetworkShare** : Network Share based implementations
- **POC.Storage.Null** : Mock implentations for the unit tests

## Test Projects
- **POC.Storage.Core** : tests for the the implementation indepent code base; contains configuration reading tests
- **POC.Storage.Elasticsearch.Tests** : tests for the Elasticsearch based implentations
- **POC.Storage.MsSql.Tests** : tests for the MsSql based implentations
- **POC.Storage.MsSql-ES-NS.Tests** : tests for a complex configuration which uses MsSql, Elasticsearch and Network Share implementations simultaneously
- **POC.Storage.NetworkShare.Tests** : tests for the Network Share based implentations
- **POC.Storage.Null.Tests** : tests for the mock implentations

# Running Unit Tests
The unit tests are using [xunit](https://xunit.net/) test framework. You can use Visual Studio's *Test Explorer* tab to run the test or execute the from console. If you have problems running the tests please read through xunit's [Getting Started](https://xunit.net/docs/getting-started/netfx/visual-studio) article.

# Running Test Importer
1. Open a command prompt
2. Go to artifacts\Debug\POC.Storage.Importer\netcoreapp3.1\
3. Execute **POC.Storage.Importer.exe help** to display arugment options
4. Execure **POC.Storage.Importer.exe** with one of the listed argument in the help screen

## Argument Options
- **create** : creates some test documents
- **create1000** : creates 1000 test documents
- **update** : updates 10000 test documents


