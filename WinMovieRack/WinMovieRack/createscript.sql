-- Creator:       MySQL Workbench 5.2.34/ExportSQLite plugin 2009.12.02
-- Author:        Dominic Rausch
-- Caption:       New Model
-- Project:       Name of the project
-- Changed:       2011-05-28 12:06
-- Created:       2011-05-27 15:16
PRAGMA foreign_keys = OFF;

-- Schema: mydb
BEGIN;
CREATE TABLE "Movies"(
  "idMovies" INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
  "Title" TEXT NOT NULL,
  "Runtime" INTEGER,
  "Plot" TEXT,
  "OriginalTitle" TEXT,
  "imdbID" CHAR(20),
  "metacriticsID" VARCHAR(255),
  "rottentomatoesID" VARCHAR(255),
  "imdbTop250" INTEGER,
  "imdbRating" INTEGER,
  "personalRating" INTEGER,
  "Year" INTEGER,
  "boxofficemojoID" VARCHAR(255),
  CONSTRAINT "metacriticsID_UNIQUE"
    UNIQUE("metacriticsID"),
  CONSTRAINT "rottentomatoesID_UNIQUE"
    UNIQUE("rottentomatoesID"),
  CONSTRAINT "boxofficemojoID_UNIQUE"
    UNIQUE("boxofficemojoID")
);
CREATE TABLE "Person"(
  "idPerson" INTEGER PRIMARY KEY NOT NULL,
  "Name" VARCHAR(255),
  "OriginalName" VARCHAR(255),
  "Biography" TEXT,
  "Birthday" DATE,
  "Birthplace" VARCHAR(255),
  "imdbRating" INTEGER,
  "myRating" INTEGER,
  "lifetimeGross" INTEGER,
  "boxofficeAverage" INTEGER
);
CREATE TABLE "Role"(
  "Person_idPerson" INTEGER NOT NULL,
  "Movies_idMovies" INTEGER NOT NULL,
  "CharacterName" VARCHAR(255),
  "Rank" INTEGER,
  PRIMARY KEY("Person_idPerson","Movies_idMovies"),
  CONSTRAINT "fk_Person_has_Movies_Person"
    FOREIGN KEY("Person_idPerson")
    REFERENCES "Person"("idPerson"),
  CONSTRAINT "fk_Person_has_Movies_Movies1"
    FOREIGN KEY("Movies_idMovies")
    REFERENCES "Movies"("idMovies")
);
CREATE INDEX "Role.fk_Person_has_Movies_Movies1" ON "Role"("Movies_idMovies");
CREATE INDEX "Role.fk_Person_has_Movies_Person" ON "Role"("Person_idPerson");
CREATE TABLE "Producer"(
  "Person_idPerson" INTEGER NOT NULL,
  "Movies_idMovies" INTEGER NOT NULL,
  PRIMARY KEY("Person_idPerson","Movies_idMovies"),
  CONSTRAINT "fk_Person_has_Movies_Person1"
    FOREIGN KEY("Person_idPerson")
    REFERENCES "Person"("idPerson"),
  CONSTRAINT "fk_Person_has_Movies_Movies2"
    FOREIGN KEY("Movies_idMovies")
    REFERENCES "Movies"("idMovies")
);
CREATE INDEX "Producer.fk_Person_has_Movies_Movies2" ON "Producer"("Movies_idMovies");
CREATE INDEX "Producer.fk_Person_has_Movies_Person1" ON "Producer"("Person_idPerson");
CREATE TABLE "Regisseur"(
  "Person_idPerson" INTEGER NOT NULL,
  "Movies_idMovies" INTEGER NOT NULL,
  PRIMARY KEY("Person_idPerson","Movies_idMovies"),
  CONSTRAINT "fk_Person_has_Movies_Person3"
    FOREIGN KEY("Person_idPerson")
    REFERENCES "Person"("idPerson"),
  CONSTRAINT "fk_Person_has_Movies_Movies4"
    FOREIGN KEY("Movies_idMovies")
    REFERENCES "Movies"("idMovies")
);
CREATE INDEX "Regisseur.fk_Person_has_Movies_Movies4" ON "Regisseur"("Movies_idMovies");
CREATE INDEX "Regisseur.fk_Person_has_Movies_Person3" ON "Regisseur"("Person_idPerson");
CREATE TABLE "MovieInstance"(
  "Movies_idMovies" INTEGER NOT NULL,
  "idMovieInstance" INTEGER PRIMARY KEY NOT NULL,
  "Runtime" INTEGER,
  "DateAdded" DATE NOT NULL,
  CONSTRAINT "fk_MovieInstance_Movies1"
    FOREIGN KEY("Movies_idMovies")
    REFERENCES "Movies"("idMovies")
);
CREATE INDEX "MovieInstance.fk_MovieInstance_Movies1" ON "MovieInstance"("Movies_idMovies");
CREATE TABLE "Locations"(
  "idLocations" INTEGER PRIMARY KEY NOT NULL,
  "Name" VARCHAR(255),
  "MovieInstance_idMovieInstance" INTEGER NOT NULL,
  CONSTRAINT "fk_Locations_MovieInstance1"
    FOREIGN KEY("MovieInstance_idMovieInstance")
    REFERENCES "MovieInstance"("idMovieInstance")
);
CREATE INDEX "Locations.fk_Locations_MovieInstance1" ON "Locations"("MovieInstance_idMovieInstance");
CREATE TABLE "Medium"(
  "idMedium" INTEGER PRIMARY KEY NOT NULL,
  "Name" VARCHAR(255),
  "MovieInstance_idMovieInstance" INTEGER NOT NULL,
  CONSTRAINT "fk_Medium_MovieInstance1"
    FOREIGN KEY("MovieInstance_idMovieInstance")
    REFERENCES "MovieInstance"("idMovieInstance")
);
CREATE INDEX "Medium.fk_Medium_MovieInstance1" ON "Medium"("MovieInstance_idMovieInstance");
CREATE TABLE "Seen"(
  "idSeen" INTEGER PRIMARY KEY NOT NULL,
  "Date" DATE,
  "Comment" TEXT,
  "Movies_idMovies" INTEGER NOT NULL,
  CONSTRAINT "fk_Seen_Movies1"
    FOREIGN KEY("Movies_idMovies")
    REFERENCES "Movies"("idMovies")
);
CREATE INDEX "Seen.fk_Seen_Movies1" ON "Seen"("Movies_idMovies");
CREATE TABLE "MoviePictures"(
  "idMoviePictures" INTEGER PRIMARY KEY NOT NULL,
  "Movies_idMovies" INTEGER NOT NULL,
  "Path" VARCHAR(255),
  CONSTRAINT "fk_MoviePictures_Movies1"
    FOREIGN KEY("Movies_idMovies")
    REFERENCES "Movies"("idMovies")
);
CREATE INDEX "MoviePictures.fk_MoviePictures_Movies1" ON "MoviePictures"("Movies_idMovies");
CREATE TABLE "PersonPictures"(
  "idPersonPictures" INTEGER PRIMARY KEY NOT NULL,
  "Path" VARCHAR(255),
  "Person_idPerson" INTEGER NOT NULL,
  CONSTRAINT "fk_PersonPictures_Person1"
    FOREIGN KEY("Person_idPerson")
    REFERENCES "Person"("idPerson")
);
CREATE INDEX "PersonPictures.fk_PersonPictures_Person1" ON "PersonPictures"("Person_idPerson");
CREATE TABLE "MovieRating"(
  "idMovieRating" INTEGER PRIMARY KEY NOT NULL,
  "Movies_idMovies" INTEGER NOT NULL,
  "rating" VARCHAR(45),
  CONSTRAINT "fk_MovieRating_Movies1"
    FOREIGN KEY("Movies_idMovies")
    REFERENCES "Movies"("idMovies")
);
CREATE INDEX "MovieRating.fk_MovieRating_Movies1" ON "MovieRating"("Movies_idMovies");
CREATE TABLE "Country"(
  "idCountry" INTEGER PRIMARY KEY NOT NULL,
  "Country" VARCHAR(45),
  CONSTRAINT "Country_UNIQUE"
    UNIQUE("Country")
);
CREATE TABLE "Country_has_Movies"(
  "Country_idCountry" INTEGER NOT NULL,
  "Movies_idMovies" INTEGER NOT NULL,
  PRIMARY KEY("Country_idCountry","Movies_idMovies"),
  CONSTRAINT "fk_Country_has_Movies_Country1"
    FOREIGN KEY("Country_idCountry")
    REFERENCES "Country"("idCountry"),
  CONSTRAINT "fk_Country_has_Movies_Movies1"
    FOREIGN KEY("Movies_idMovies")
    REFERENCES "Movies"("idMovies")
);
CREATE INDEX "Country_has_Movies.fk_Country_has_Movies_Movies1" ON "Country_has_Movies"("Movies_idMovies");
CREATE INDEX "Country_has_Movies.fk_Country_has_Movies_Country1" ON "Country_has_Movies"("Country_idCountry");
COMMIT;
