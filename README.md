
[![Test Run](https://github.com/ValentinShikovIT/FullFraim/actions/workflows/dotnet.yml/badge.svg)](https://github.com/ValentinShikovIT/FullFraim/actions/workflows/dotnet.yml)

---

#  [**FullFraim**](https://fullfraim.azurewebsites.net/)
## _A site full of photo-contests in which you can participate and earn the title of "Photo Dictator"_
## _Cool Right!_
![N|Solid](https://res.cloudinary.com/fullfraim/image/upload/v1623086529/For%20Documentation/logo_gptfjh.png)

## Description
> FullFraim is a project written in ASP.NET. It Includes a public API and a MVC.
> The site features photo-contests in which a newly registered user can participate, starting from the beginning with rank "Photo Junkie"
> Once a thresholed of points is surpassed the user can advance in rank until he or she can become a jury and participate as such
> For more information on functionality, [down in the document]()

---

#### Dashboard Page
![N|Solid](https://res.cloudinary.com/fullfraim/image/upload/v1623093475/For%20Documentation/LandingPage_rmesdh.png)
### Register Page
![N|Solid](https://res.cloudinary.com/fullfraim/image/upload/v1623093475/For%20Documentation/RegisterPage_dr3hru.png)
### Login Page
![N|Solid](https://res.cloudinary.com/fullfraim/image/upload/v1623093474/For%20Documentation/LoginPage_bo2rgo.png)
### Logged user sees
![N|Solid](https://res.cloudinary.com/fullfraim/image/upload/v1623093475/For%20Documentation/LoggedUserSees_z1et0q.png)
### Dashboard Page
![N|Solid](https://res.cloudinary.com/fullfraim/image/upload/v1623093475/For%20Documentation/DashBoardPage_dacz65.png)
### Create Contest Form
![N|Solid](https://res.cloudinary.com/fullfraim/image/upload/v1623093474/For%20Documentation/CreateContest_grqlvh.png)
### Users Page
![N|Solid](https://res.cloudinary.com/fullfraim/image/upload/v1623093475/For%20Documentation/UsersPage_vnp5wy.png)

---

## Features - MVC

> ### Landing page (visible to everyone)
```sh
> The landing page displays the top, ranked highest photos. 
> If you are not part of our society yet, you have the possibility to register and then login and start participating.
> Upon registration you will be asked to confirm your credientials by accepting the confirmation email.
```
> ### Dashboard page (visible only to logged users)
FOR ORGANIZERS
```sh
> They can create new contests.
> They can view in which phase a contest is (open, in reviewing, finished)
> Then can view all photo junkies registered on the website.
```
FOR PHOTO JUNKIES
```sh
> They can view Open contests in which they can participate.
> They can view the contests that they participate in.
> They can view their current score and how much points are required to reach the next rank.
```
> ### Contest page (visible only to logged users)
FOR CONTESTS IN PHASE ONE
```sh
> You have the ability to view the remaining time until the contest reaches Phase Two.
> "JURY" can view the submitted photos but cannot rate them yet.
> You will see the "Enroll" button if you are not currently enrolled to participate in the current contest
> To participate, you will need to fill
-- Title for the image (short text)
-- Story for the image (long text)
-- Photo with which to participate (be careful of the contest category)
> A user can only submit one photo per contest.
> Once a photo is submitted, it cannot be edited
```
FOR CONTESTS IN PHASE TWO
```sh
> You have the ability to view the remaining time until the contest reaches "Finished" Phase.
> Participants cannot upload anymore, therefore the "Enroll" button is no longer active.
> "JURY" sees a form for each submitted photo
-- Score (1-10)
-- Comment (long text)
-- Checkbox to mark that the photo does not fit the contest category (If it doesn t)
> "JURY" can only give one review per photo
```
FOR CONTESTS IN FINISHED PHASE
```sh
> "JURY" can no longer review photos.
> Participants view their score and comments.
> In this phase, participants can also view the photos submitted by other users, along with their socres and comments by the jury.
```
> ### Create contest Form (visible only to Organizers or above)
```sh
-- Title (text field)
-- Category
-- Open or Invitational
-- Time Span of Phase One
-- Time Span of Phase Two
-- Select Jury (all Organizers are automatically selected as a jury)
-- Cover Photo for the Contest (with three options 1. upload 2. with URL 3. Select from previous)
```
> ### Scoring (Implementation)
```sh
> Joining an open contest - 1 points
> Being invited by organizer – 3 points
> 3rd place – 20 points (10 points if shared 3rd)
> 2nd place – 35 points (25 points if shared 2nd)
> 1st place – 50 points (40 points if shared 1st)
> Finishing at 1st place with double the score of the 2nd (e.g., 1st has been awarded 8.6
points average, and 2nd is 4.3 or less) – 75 points
> In case of a tie, positions are shared, so there can be more than one participant at 1st, 2nd,
and 3rd places, all in the same contest.
> For example, two 1st places, one 2nd and four 3rds; the two winners will each get 40 points, the
only 2nd place will get the full 35 points, and the four 3rd finishers will get 10 points.
```
> ### Ranking (Implementation)
```sh
> (0-50) points – Junkie
> (51 – 150) points – Enthusiast
> (151 – 1000) points – Master (can now be invited as jury)
> (1001 – infinity) points – Wise and Benevolent Photo Dictator (can still be jury)
```
---
# _TAKE A LOOK AT SOME OF OUR PHOTOS_
![N|Solid](https://res.cloudinary.com/fullfraim/image/upload/v1623056700/zsnzlycu4kbo5hzqqgcg.jpg)

![N|Solid](https://res.cloudinary.com/fullfraim/image/upload/v1623052957/gmwexkb2metebo6bv5lt.jpg)

![N|Solid](https://res.cloudinary.com/fullfraim/image/upload/v1623059361/nswjb50nw2pksire5lre.jpg)

---

## Features - API

> API IMPLEMENTED USING SWAGGER!

![N|Solid](https://res.cloudinary.com/fullfraim/image/upload/v1623091505/For%20Documentation/swaggerPresentation_z1qwgy.png)

> ### Account (used for registration and login)
```sh
> GET: "/api/Account/Login" => used to login and receive the JWT Token
> POST: "/api/Account/Register" => used to register in our system
```
> ### Contests (used to CRUD contests)
```sh
> GET: "/api/Contests" => used to get all contests (implements query filters and pagination)
> POST: "/api/Contests" => used to create a contest
> GET: "/api/Contests/{contestId}" => used to get a contest by id
> PUT: "/api/Contests/{id}" => used to update a contest
> DELETE: "/api/Contests/{id}" => used to delete a contests
> GET: "Covers" => used to get all available covers
```
> ### Dashboard
```sh
> GET: "/api/Dashboard" => used to get contest without additional information
```
> ### Junkies
```sh
> GET: "/api/Junkies" => used to get all junkies
> POST: "/api/Junkies/enroll" => used to enroll onto a given contest
> GET: "/api/Junkies/nextrank" => used to get the points till next rank of the junkie
```
> ### Juries
```sh
> GET: "/api/Juries/review" => used to get all reviews of one jury
```
> ### Photos
```sh
> GET: "/api//Photos" => used to get all photos for a given contest
> POST: "/api/Photos/submissions" => used to get all submissions for the given contest
> GET: "/api/Photos/{id}" => used to get a photo by id
> GET: "/api/Photos/TopRecent" => used to get the top recent photos out of all
```
---
## Installation
> If you wish to download the app follow the steps below
> If you find some improvements, feel free to contact us at fullfraim@gmail.com
> HAVE FUN!
```sh
> Download the app from the repository
> Add the connection string to your database
> Run the application. The database will be automatically created
```

### Database Diagram
---
![N|Solid](https://res.cloudinary.com/fullfraim/image/upload/v1623086127/For%20Documentation/fullfraimDiagran_itmphl.jpg)

## Technologies used

FullFraim uses a number of open source projects to work properly:

- ASP.NET Core 3.1
 - Microsoft Entity Framework Core 3.1
 - MSSQL
 - Moq
 - Swagger
 - EF Core InMemory
 - Serilog
 - JavaScript
 - AJAX
 - JQuery
 - Cloudinary
 - JWT Bearer
 - ASP.NET Identity
 - Azure Deployment
 - Razor Pages
 - ViewComponents
 - Html 5
 - CSS 3
 - SendGrid

## Contacts

Contact us for further information

| Contacts | Emails |
| ------ | ------ |
| Ivan Dichev | i.dichev0@gmail.com |
| Boryana Mihaylova | boryana.gm@gmail.com |
| Valentin Shikov | valentinshikov@abv.bg |
