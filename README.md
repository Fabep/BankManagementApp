# BankAssignment

Check out the bank management app [here](bankcool.azurewebsites.net/)! To login use these accounts,

Admin - Username: test@admin.com , Password: Test123!

Cashier - Username: test@cashier.com , Password: Test123!


This project is designed to simulate a web bank application. By using the principles of database first, the database in this project has been scaffolded. 
There are certain criteria's that have been met to certify our customers satisfaction. If you want to use this web application locally, feel free to download the original database [here](https://aspcodeprod.blob.core.windows.net/school-dev/BankAppDatav2%20(1).bak).

# Application

The landing page of the project shows some statistics about the project. There's also links to see the top 10 accounts in each of the countries. Once you log in as a cashier you'll be able to see the customer page, this page lets you see a list of all the customers. There is a single button for each of the customers that takes you to their overview page. Here you'll see more information about the customer and all their accounts. Click on one of the accounts and you'll get to a new page where you'll be able to see account specific information. As well as be able to make withdrawals, deposits and transfer money to other accounts within the bank app. 

# Infrastructure

Due to the large amount of customers in the database [this](https://www.codingame.com/playgrounds/5363/paging-with-entity-framework-core) class has been used for creating a pagination in lists where customers are shown. 

Automapper have been used throughtout the project to convert objects from database objects to viewmodels and vice versa. 

# Services

With dependency injection all the services have been injected and are available throughout the project. 

## Account Service

All the methods in the account service are there to support and read accounts. 

## Customer Service

All the methods in the customer service are there to support, read, create and update customers. 

## Disposition Service

This service only exists for one purpose and that is to create a connection when between a customer and an account once a customer has been created. 

## Sort Service

This should be a generic method handler and not a service. The method in this service only exists to help show the small arrows used for sorting at the customer page. 

## Transaction Service

Handles everything involving transactions. Right now I've decided to handle everything revolving making transactions in one method, this should've been split up into three submethods being called from the major make transaction one. 

## Admin Service

Admin service allows admins of the web application to create and update users of the database. 

# Bank Api

The bank api is made for a mobile application, the mobile application doesn't exist in this case. But if it were to it would let customers get their own information and transactions for an account they own. Optimally this would be shown automatically in the application once you've logged in to it.

# Money laundring check

The suspicious money console app is an app made to check whether or not a customer has made suspicious transactions. This app is made to reuse the code that I've made in both the web application and the api.
