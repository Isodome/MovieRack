﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace WinMovieRack.Model
{
    public class ImdbMovie
    {
		public uint imdbID;
		public string title;
		public int year;
		public int runtime;
		public string originalTitle;
		public List<string> genres;
		public string plot;
		public int imdbRating;
		public int imdbRatingVotes;
		public List<string> countries;
		public List<string> languages;
		public List<uint> directors;
		public List<uint> writers;
		public List<uint> stars;
		public List<Tuple<uint, string>> roles;
		public string alsoKnownAs;
		public Bitmap poster = null;
		public List<Award> awards;

		public ImdbMovie() {

			genres = new List<string>();
			countries = new List<string>();
			languages = new List<string>();
			directors = new List<uint>();
			writers = new List<uint>();
			stars = new List<uint>();
			roles = new List<Tuple<uint, string>>();
			awards = new List<Award>();
		}

		public ImdbMovie(uint imdbID) : this() {
			this.imdbID = imdbID;
		}

		public void printToConsole() {
			Console.WriteLine("imdbID: " + this.imdbID);
			Console.WriteLine("Title: " + this.title);
			Console.WriteLine("Original Title: " + this.originalTitle);
			Console.WriteLine("Year:" + this.year);
			Console.WriteLine("Runtime: " + this.runtime);
			Console.WriteLine("Plot: " + this.plot);
			Console.Write("Genres: ");
			foreach (string s in genres) {
				Console.Write(s + ", ");
			}
			Console.WriteLine("");
			Console.WriteLine("imdb Rating(*10): " + this.imdbRating);
			Console.WriteLine("Votes: " + this.imdbRatingVotes);
			Console.Write("Countries: ");
			foreach (string s in countries) {
				Console.Write(s + ", ");
			}
			Console.WriteLine("");
			Console.Write("Languages: ");
			foreach (string s in languages) {
				Console.Write(s + ", ");
			}
			Console.WriteLine("");
			Console.WriteLine("Also known as: " + this.alsoKnownAs);
			Console.Write("Directors: ");
			foreach (uint s in directors) {
				Console.Write(s.ToString() + ", ");
			}
			Console.WriteLine("");
			Console.Write("Writers: ");
			foreach (uint s in writers) {
				Console.Write(s.ToString() + ", ");
			}
			Console.WriteLine("");

			Console.Write("Cast: ");
			foreach (Tuple<uint, string> t in roles.ToArray<Tuple<uint, string>>()) {
				Console.WriteLine(t.Item1.ToString() + " as " + t.Item2);
			}
			Console.Write("Stars: ");
			foreach (uint s in stars) {
				Console.Write(s.ToString() + ", ");
			}
			Console.WriteLine("Awards:");
			foreach (Award a in awards) {
				Console.WriteLine(a.ToString());
			}
		}
    }
}
