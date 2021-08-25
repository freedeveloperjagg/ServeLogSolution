# ServeLogSolution
Logger as Service

This service logger allow you to manage your logger system through a web service. given you a great flexibility if you system consist of several application tht need to log. 
Some advantages are:
* Only need communication with the servelog throug port 80
* You can use it directly, but if you want to control level of logger need to create a client.
* The actual version of servelog is in NET 5.0.
* Because the access is trhough HTTP protocol the client applications can be in any OS.

# Requisites

* ServeLog is tailored to use Microsoft SQL Server to store the information. Then you need it
* We are testing it using IIS, but you can install it whatever web server that support NET 5.0

# Installation

* Create first the necessary Dstabases. By default you need to create the Table para el internal log
* It is recomendable create a DB specific for all Client tables.
* The internal table by default is named TestLog, but you can use other name if you update the appsettings.json according
* Use the script in the code DataBaseScript.sql to generate your tables. Configure el script before use it.



# Documentation

