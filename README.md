# Modisette.com

## Overview

The project is my capstone project for CODE: You. The application will serve as a personal website for a client. There is an admin section where she can review contact submissions and add/edit (CRUD) courses and the course content that she uploads. Uploaded files are stored in the wwwroot folder and served up to users through the content page by virtue of their URI. As for contact submissions, beyond the searchable admin dashboard that shows contact submissions, when a contact form is submitted, a version of that message is sent to a dummy email through Google's Smtp. The website uses AuthO (an implementation of OAuth 2.0) to handle authentication and authorization. Within AuthO, I have written a Node.js script that authorizes only certain accounts to access the admin portion of the site.   

## Features Utilized for the project

  | Feature        | Description                           |
  |----------------|---------------------------------------|
  | C#, HTML, CSS | Front end UI was made using ASP.NET Core's Razor Pages. Back end was written in C#. There are also two brief JavaScript functions for improved UX. |
  | Entity Framework Core | The data layer is abstracted with EF Core as an ORM. There is a one-to-many relationship between courses and course documents, and there is a composite primary key in the courses table. |
  | Unit Tests | The project includes 7 unit tests which cover all the basic functions of the site. |
  | Asynchronous Methods | All of the main methods used in the program are asynchronous. |
  | Responsive Design | All pages are built with a responsive design in mind and will work on mobile and desktop devices. |
  | Complex queries | The content page has a series of selections which pull data from two different tables in the database. |
   
## Database

All data for the site is store in a SQLite database named Modisette.db.

## Dependencies

> The project targets .NET 8.0, and that version of .NET will need to be installed for the site to load properly. 
