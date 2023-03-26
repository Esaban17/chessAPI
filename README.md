<p align="center">
  <a href="" rel="noopener">
 <img height=200px src="https://github.com/jsamayoap/chessAPI/blob/master/mychessgame.jpg" alt="Project logo"></a>
</p>

<h3 align="center">Chess API</h3>

<div align="center">

[![Status](https://img.shields.io/badge/status-active-success.svg)]()
[![GitHub Issues](https://img.shields.io/github/issues/kylelobo/The-Documentation-Compendium.svg)](https://github.com/jsamayoap/chessAPI/issues)
[![GitHub Pull Requests](https://img.shields.io/github/issues-pr/kylelobo/The-Documentation-Compendium.svg)](https://github.com/jsamayoap/chessAPI/pulls)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](/LICENSE)

</div>

---

<p align="center"> This a REST application to serve the chess game backend services.
    <br> 
</p>

## 📝 Table of Contents

- [About](#about)
- [Getting Started](#getting_started)
- [Deployment](#deployment)
- [Usage](#usage)
- [Built Using](#built_using)
- [TODO](../TODO.md)
- [Contributing](../CONTRIBUTING.md)
- [Authors](#authors)
- [Acknowledgments](#acknowledgement)

## 🧐 About <a name = "about"></a>

This project pretends to be a starting point for our Software Engineering II course.

## 🏁 Getting Started <a name = "getting_started"></a>

Fork this repo and start coding. See [deployment](#deployment) for notes on how to deploy the project on a live system.

### Prerequisites

On our local environment you will need:
- [.Net 7 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/7.0)
- Docker

### Installing

To be able to run locally your code follow these instructions.

1. Install the pre-requisites
2. Trust the cert development certificate (in order to get https access to your API)
```
dotnet dev-certs https --trust
```
3. Create docker volumes for the database and adminUI images

```
docker volume create pg_main_data
docker volume create pga4data
docker volume create mongoData
```
This will install the needed dependencies to store our API, the default database is PostgreSQL but feel free to adjust the app to whatever best suits you for a development database.

4. Start the containers

```
docker-compose up -d
```

## 🔧 Running the tests <a name = "tests"></a>

TBD.

## 🎈 Usage <a name="usage"></a>

Add notes about how to use the system.

## 🚀 Deployment <a name = "deployment"></a>

Add additional notes about how to deploy this on a live system.

## ⛏️ Built Using <a name = "built_using"></a>

- [Autofac](https://autofac.org/)
- [Dapper](https://github.com/DapperLib/Dapper)
- [Serilog](https://serilog.net/)
- [PostgreSQL](https://www.postgresql.org/) - Relational Database
- [MongoDB](https://www.mongodb.com/) - Non Relational Database
- [ASP.Net](https://dotnet.microsoft.com/en-US/apps/aspnet) - Server Framework
- [.Net 7](https://dotnet.microsoft.com/en-US/) - Server Environment

## ✍️ Authors <a name = "authors"></a>

- [@jsamayoap](https://github.com/jsamayoap) - Idea & Initial work

See also the list of [contributors](https://github.com/jsamayoap/The-Documentation-Compendium/contributors) who participated in this project.

## 🎉 Acknowledgements <a name = "acknowledgement"></a>

- Hat tip to anyone whose code was used
- Inspiration
- References
