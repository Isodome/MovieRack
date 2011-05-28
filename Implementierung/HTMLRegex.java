import java.awt.Image;
import java.io.BufferedReader;
import java.io.File;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.net.MalformedURLException;
import java.net.URL;
import java.net.URLConnection;
import java.util.ArrayList;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

public class HTMLRegex {

	String sucheFilm = "Metropolis";

	static String movieID;
	static String titlePlusJahr = "<title>(.*?)\\s\\((.*?)\\) -";
	static String charaktere = "<td class=\"character\">.*?<a\\s+href=\"/character/ch(.*?)/\">(.*?)</a>";
	static String schauspieler = "<td class=\"name\">\\s+<a\\s+href=\"/name/nm(.*?)/\">(.*?)</a>";
	static String storyline = "<h2>Storyline</h2>\\s+<p>(.*?)<em";
	static String premiereDaten = "<a\\s+href=\"/calendar/.*?>(.*?)<.*?<a href=\".*?>(.*?)<.*?href=.*?>(.*?)<";
	static String wertung = "Users:.*?<b>(.*?)<.*?title=.*?>(.*?)<";
	static String fullSchauspieler = "width=\"2.*?;\">(.*?)<.*?char\">(.*?)<.*?>(.*?)<";
	static String poster = "image_src\" href=\"(.*?)\">";
	static String posterURLLowQuality = "<div\\s+class=\"poster\">.*?\"(.*?)\"";
	static String bilderURL = "<td rowspan=\"2\" id=\"img_primary\">.*? href=\"(.*?)\"";
	static String trailer = "id=\"overview-bottom\">.*?a\\s+href=\"(.*?)\"";
	static String boxoffice = "Domestic:.*?<b>(.*?)<.*?Foreign:.*?&nbsp;(.*?)<.*?Worldwide:.*?<b>(.*?)<";
	static String wertungMobile = "<strong>(.*?)<.*?&#40;(.*?)\\s+votes";
	static String actorPicture = "<div id=\"main\">.*?src=\"(.*?)\"";
	static String actorPictureMobileLowQuality = "<section\\s+class=\"overview\">.*?src=\"(.*?)\"";
	static String actorPictureMobile = "class=\"overview\">.*?href=\"(.*?)\"";
	static String imdbSuche = "Popular\\sTitles.*?href=\"/title/(.*?)/";
	static String ofdbSuchergebniss = "<a href=\"f(.*?)\">(.*?)<";
	static String ofdbNote = "images/notenspalte0.gif.*?<br>(.*?)&nbsp;";
	static String ffaZahlen = "headers=\"1\">(.*?)\\s\\(.*?headers=\"6\".*?>(.*?)<";
	static String boxofficeSuche = "Movie\\sMatches.*?<a\\shref=\"(.*?)\"";
	static String imdbID = "content=\"h.*?title/(.*?)/";

	Pattern schauspielerPatt = Pattern.compile(schauspieler, Pattern.DOTALL
			| Pattern.UNIX_LINES);
	Pattern charakterePatt = Pattern.compile(charaktere, Pattern.DOTALL
			| Pattern.UNIX_LINES);
	Pattern storylinePatt = Pattern.compile(storyline, Pattern.DOTALL
			| Pattern.UNIX_LINES);
	Pattern premiereDatenPatt = Pattern.compile(premiereDaten, Pattern.DOTALL
			| Pattern.UNIX_LINES);
	Pattern wertungPatt = Pattern.compile(wertung, Pattern.DOTALL
			| Pattern.UNIX_LINES);
	Pattern fullSchauspielerPatt = Pattern.compile(fullSchauspieler,
			Pattern.DOTALL | Pattern.UNIX_LINES);
	Pattern posterURLPatt = Pattern.compile(poster, Pattern.DOTALL
			| Pattern.UNIX_LINES);
	Pattern bilderURLPatt = Pattern.compile(bilderURL, Pattern.DOTALL
			| Pattern.UNIX_LINES);
	Pattern titlePatt = Pattern.compile(titlePlusJahr, Pattern.DOTALL
			| Pattern.UNIX_LINES);
	Pattern trailerPatt = Pattern.compile(trailer, Pattern.DOTALL
			| Pattern.UNIX_LINES);
	Pattern boxofficePatt = Pattern.compile(boxoffice, Pattern.DOTALL
			| Pattern.UNIX_LINES);
	Pattern wertungMobilePatt = Pattern.compile(wertungMobile, Pattern.DOTALL
			| Pattern.UNIX_LINES);
	Pattern posterURLLowQualityPatt = Pattern.compile(posterURLLowQuality,
			Pattern.DOTALL | Pattern.UNIX_LINES);
	Pattern actorPicturePatt = Pattern.compile(actorPicture, Pattern.DOTALL
			| Pattern.UNIX_LINES);
	Pattern actorPictureMobileLowQualityPatt = Pattern.compile(
			actorPictureMobileLowQuality, Pattern.DOTALL | Pattern.UNIX_LINES);
	Pattern actorPictureMobilePatt = Pattern.compile(actorPictureMobile,
			Pattern.DOTALL | Pattern.UNIX_LINES);
	Pattern ffaZahlenePatt = Pattern.compile(ffaZahlen, Pattern.DOTALL
			| Pattern.UNIX_LINES);
	Pattern ofdbSuchergebnissPatt = Pattern.compile(ofdbSuchergebniss,
			Pattern.DOTALL | Pattern.UNIX_LINES);
	Pattern ofdbNotePatt = Pattern.compile(ofdbNote, Pattern.DOTALL
			| Pattern.UNIX_LINES);
	Pattern boxofficeSuchePatt = Pattern.compile(boxofficeSuche, Pattern.DOTALL
			| Pattern.UNIX_LINES);
	Pattern imdbSuchePatt = Pattern.compile(imdbSuche, Pattern.DOTALL
			| Pattern.UNIX_LINES);
	Pattern imdbIDPatt = Pattern.compile(imdbID, Pattern.DOTALL
			| Pattern.UNIX_LINES);

	String ofdbFilm = "http://www.ofdb.de/";
	String imdbURL = "http://www.imdb.com";
	String imdbSearchURL = "http://www.imdb.com/find?s=all&q=";
	String boxofficeSearchString = "http://boxofficemojo.com/search/?q=";
	URL ofdbSearchURL;
	URL ffaURL;
	URL ffaURLpuls1;
	URL posterURL = null;
	URL boxofficeSearch;
	URL filminfoURL;
	URL premiereDatenURL;
	URL wertungMobileURL;
	URL fullCreditsURL;

	String boxofficeTitleString;
	String titleString = null;
	String jahr = null;

	CharSequence ofdbSuchergebnissChar;
	CharSequence filminfoChar;
	CharSequence boxofficeChar;
	CharSequence premiereDatenChar;
	CharSequence fullCreditsChar;
	CharSequence wertungMobileChar;
	CharSequence posterURLLowQualityChar;
	CharSequence boxofficeSucheChar;

	public HTMLRegex() throws IOException {

		imdbTitle();
		ofdb();
		imdb();
		imdbFullCast();
		imdbMobileWertung();
		imdbPosterHighQulaity();
		imdbPosterLowQuality();
		// imdbTrailer();
		// ffaListe();
		imdbSuche();
		boxofficemojoSuche();
		boxofficemojo();
	}

	public void ofdb() throws IOException {
		ofdbSearchURL = new URL(
				"http://www.ofdb.de/view.php?page=erwblaettern&Kat=Film&MF=N&Titel="
						+ sucheFilm
						+ "&Darsteller=&Regie=&Land=-&Alter=-&Genre=-&Inhalt=&Submit=Suche+ausf%C3%BChren");
		if (ofdbSuchergebnissChar == null) {
			ofdbSuchergebnissChar = getURLContent(ofdbSearchURL);
		}
		System.out.println("OFDB\n");
		Matcher matcher = ofdbSuchergebnissPatt.matcher(ofdbSuchergebnissChar);
		ArrayList<String> ofdbErgebnisseTitel = new ArrayList<String>();
		ArrayList<String> ofdbErgebnisseLinks = new ArrayList<String>();
		ArrayList<String> ofdbErgebnisseTitelParsed = new ArrayList<String>();
		ArrayList<String> ofdbErgebnisseJahre = new ArrayList<String>();
		while (matcher.find()) {
			ofdbErgebnisseLinks.add(ofdbFilm + "f" + matcher.group(1));
			ofdbErgebnisseTitel.add(matcher.group(2));
		}

		String titelTemp = "";
		String jahrTemp = "";
		for (int i = 0; i < ofdbErgebnisseTitel.size(); i++) {
			String[] resultTitle = ofdbErgebnisseTitel.get(i).split("\\(");

			if (resultTitle.length > 2) {
				for (int j = 0; j < resultTitle.length - 1; j++) {
					if (j > 0) {
						titelTemp = titelTemp + "(" + resultTitle[j];
					} else {
						titelTemp = titelTemp + resultTitle[j];
					}
				}
				titelTemp = titelTemp.trim();
				ofdbErgebnisseTitelParsed.add(titelTemp);
				jahrTemp = resultTitle[resultTitle.length - 1];
				String[] resultJahr = jahrTemp.split("\\)");
				jahrTemp = resultJahr[0];
				jahrTemp = jahrTemp.trim();
				ofdbErgebnisseJahre.add(jahrTemp);
			} else {
				titelTemp = resultTitle[0];
				jahrTemp = resultTitle[1];
				String[] resultJahr = jahrTemp.split("\\)");
				jahrTemp = resultJahr[0];
				titelTemp = titelTemp.trim();
				jahrTemp = jahrTemp.trim();
				ofdbErgebnisseTitelParsed.add(titelTemp);
				ofdbErgebnisseJahre.add(jahrTemp);
			}
		}
		for (int i = 0; i < ofdbErgebnisseTitelParsed.size(); i++) {
			System.out.println(ofdbErgebnisseTitelParsed.get(i) + " - "
					+ ofdbErgebnisseJahre.get(i));
		}
		for (int i = 0; i < ofdbErgebnisseLinks.size(); i++) {
			CharSequence ergebnissChar = getURLContent(new URL(
					ofdbErgebnisseLinks.get(i)));
			Matcher matcherNote = ofdbNotePatt.matcher(ergebnissChar);
			while (matcherNote.find()) {
				System.out.println(ofdbErgebnisseTitelParsed.get(i) + " - "
						+ matcherNote.group(1));
			}
		}

	}

	public void imdb() throws IOException {
		if (movieID == null) {
			imdbSuche();
		}

		if (premiereDatenURL == null) {
			premiereDatenURL = new URL("http://www.imdb.com/title/" + movieID
					+ "/releaseinfo");
		}

		if (filminfoURL == null) {
			filminfoURL = new URL("http://www.imdb.com/title/" + movieID + "/");
		}

		if (filminfoChar == null) {
			filminfoChar = getURLContent(filminfoURL);
		}
		if (premiereDatenChar == null) {
			premiereDatenChar = getURLContent(premiereDatenURL);
		}

		Matcher matcher;

		matcher = schauspielerPatt.matcher(filminfoChar);
		System.out.println("\nID - Schauspieler\n");
		while (matcher.find()) {
			System.out.println(matcher.group(1) + " - " + matcher.group(2));
		}
		System.out.println("\nID - Charakter\n");
		matcher = charakterePatt.matcher(filminfoChar);
		while (matcher.find()) {
			System.out.println(matcher.group(1) + " - " + matcher.group(2));
		}
		System.out.println("\nWertung - Votes\n");
		matcher = wertungPatt.matcher(filminfoChar);
		while (matcher.find()) {
			System.out.println(matcher.group(1) + " - " + matcher.group(2));
		}

		System.out.println("\nStoryline\n");
		matcher = storylinePatt.matcher(filminfoChar);
		while (matcher.find()) {
			System.out.println(matcher.group(1));
		}
		System.out.println("\nLand - Tag - Monat - Jahr\n");
		matcher = premiereDatenPatt.matcher(premiereDatenChar);
		while (matcher.find()) {
			System.out.println(matcher.group(1) + " - " + matcher.group(2)
					+ " - " + matcher.group(3));
		}

	}

	public void imdbActorBilder() throws IOException {
		if (filminfoChar == null) {
			filminfoChar = getURLContent(filminfoURL);
		}
		Matcher matcher = schauspielerPatt.matcher(filminfoChar);
		System.out.println("\nID - Schauspieler\n");
		while (matcher.find()) {
			System.out.println(matcher.group(1) + " - " + matcher.group(2));
			actors(matcher.group(1));
		}
	}

	public void imdbFullCast() throws IOException {
		if (movieID == null) {
			imdbSuche();
		}

		if (fullCreditsURL == null) {
			fullCreditsURL = new URL("http://www.imdb.com/title/" + movieID
					+ "/fullcredits");
		}
		if (fullCreditsChar == null) {
			fullCreditsChar = getURLContent(fullCreditsURL);
		}
		System.out.println("\nFullCast - Schauspieler - Rolle\n");
		String fullCreditsString = fullCreditsChar.toString();
		String[] fullCreditsArray = fullCreditsString.split("Produced by");
		Matcher m1 = fullSchauspielerPatt.matcher(fullCreditsArray[0]);
		while (m1.find()) {
			String output = m1.group(1) + " - " + m1.group(2) + m1.group(3);
			output = output.replaceAll("(&#x27;)", "\'");
			System.out.println(output);
		}
	}

	public void imdbMobileWertung() throws IOException {
		if (movieID == null) {
			imdbSuche();
		}

		if (wertungMobileURL == null) {
			wertungMobileURL = new URL("http://m.imdb.com/title/" + movieID
					+ "/");
		}
		if (wertungMobileChar == null) {
			wertungMobileChar = getURLContent(wertungMobileURL);
		}
		System.out.println("IMDBMobile Wertung:\n");
		Matcher m2 = wertungMobilePatt.matcher(wertungMobileChar);
		while (m2.find()) {
			System.out.println(m2.group(1) + " " + m2.group(2));
		}
	}

	public void imdbPosterHighQulaity() throws IOException {
		if (filminfoChar == null) {
			filminfoChar = getURLContent(filminfoURL);
		}

		Matcher m1 = bilderURLPatt.matcher(filminfoChar);
		while (m1.find()) {
			posterURL = new URL(imdbURL + m1.group(1));
		}

		if (posterURL != null) {
			CharSequence posterChar = getURLContent(posterURL);
			Matcher posterMatcher = posterURLPatt.matcher(posterChar);
			if (titleString == null) {
				imdbTitle();
			}
			while (posterMatcher.find()) {
				String bildURLString = posterMatcher.group(1);
				URL bildURL = new URL(bildURLString);
				InputStream in = bildURL.openStream();
				byte[] buffer = new byte[8192];
				if (titleString == null) {
					imdbTitle();
				}
				if (titleString != null) {
					File folder = new File("D:\\" + titleString
							+ File.separator);
					folder.mkdir();
					FileOutputStream out = new FileOutputStream(new File("D:\\"
							+ titleString + File.separator + titleString
							+ ".jpg"));
					int _tmp = 0;
					while ((_tmp = in.read(buffer)) > 0) {
						out.write(buffer, 0, _tmp);
					}
					out.close();
				} else {
					File folder = new File("D:\\" + "Unkown" + File.separator);
					folder.mkdir();
					FileOutputStream out = new FileOutputStream(new File("D:\\"
							+ "Unkown" + File.separator + "Unkown" + ".jpg"));
					int _tmp = 0;
					while ((_tmp = in.read(buffer)) > 0) {
						out.write(buffer, 0, _tmp);
					}
					out.close();
				}

			}
		} else {
			System.err.println("Poster nicht gefunden");
		}
	}

	public void imdbPosterLowQuality() throws IOException {
		if (movieID == null) {
			imdbSuche();
		}

		if (wertungMobileURL == null) {
			wertungMobileURL = new URL("http://m.imdb.com/title/" + movieID
					+ "/");
		}
		if (posterURLLowQualityChar == null) {
			posterURLLowQualityChar = getURLContent(wertungMobileURL);
		}
		Matcher m2 = posterURLLowQualityPatt.matcher(posterURLLowQualityChar);
		while (m2.find()) {
			String bildURLString = m2.group(1);
			URL bildURL = new URL(bildURLString);
			InputStream in = bildURL.openStream();
			byte[] buffer = new byte[8192];
			if (titleString == null) {
				imdbTitle();
			}
			if (titleString != null) {
				File folder = new File("D:\\" + titleString + File.separator);
				folder.mkdir();
				FileOutputStream out = new FileOutputStream(new File("D:\\"
						+ titleString + File.separator + titleString
						+ " - LowQuality" + ".jpg"));
				int _tmp = 0;
				while ((_tmp = in.read(buffer)) > 0) {
					out.write(buffer, 0, _tmp);
				}
				out.close();
			} else {
				File folder = new File("D:\\" + "Unkown" + File.separator);
				folder.mkdir();
				FileOutputStream out = new FileOutputStream(new File("D:\\"
						+ "Unkown" + File.separator + "Unkown - LowQuality"
						+ ".jpg"));
				int _tmp = 0;
				while ((_tmp = in.read(buffer)) > 0) {
					out.write(buffer, 0, _tmp);
				}
				out.close();
			}
		}
	}

	public void imdbTrailer() throws IOException {
		if (filminfoChar == null) {
			filminfoChar = getURLContent(filminfoURL);
		}
		Matcher m1 = trailerPatt.matcher(filminfoChar);
		while (m1.find()) {
			new ProcessBuilder(new String[] { "cmd", "/c", "start",
					imdbURL + m1.group(1) }).start();
		}
	}

	public void imdbTitle() throws IOException {
		if (movieID == null) {
			imdbSuche();
		}
		if (filminfoURL == null) {
			filminfoURL = new URL("http://www.imdb.com/title/" + movieID + "/");
		}
		if (filminfoChar == null) {
			filminfoChar = getURLContent(filminfoURL);
		}
		System.out.println("Title:\n");
		Matcher m1 = titlePatt.matcher(filminfoChar);
		while (m1.find()) {
			System.out.println(m1.group(1) + " Jahr: " + m1.group(2) + "\n");
			titleString = m1.group(1);
			jahr = m1.group(2);
			Integer jahrInt = Integer.parseInt(jahr);
		}
	}

	public void imdbSuche() throws IOException {
		sucheFilm = sucheFilm.replaceAll("\\s", "\\%20");
		URL imdbSuche = new URL(imdbSearchURL + sucheFilm);
		CharSequence imdbSucheChar = getURLContent(imdbSuche);
		Matcher m1 = imdbSuchePatt.matcher(imdbSucheChar);
		while (m1.find()) {
			movieID = m1.group(1);
		}
		if (movieID == null) {
			m1 = imdbIDPatt.matcher(imdbSucheChar);
			while (m1.find()) {
				movieID = m1.group(1);
			}
		}
	}

	public void boxofficemojo() throws IOException {
		if (boxofficeTitleString == null) {
			boxofficemojoSuche();
		}
		URL boxofficeURL = new URL("http://boxofficemojo.com"
				+ boxofficeTitleString);
		if (boxofficeChar == null) {
			boxofficeChar = getURLContent(boxofficeURL);
		}
		Matcher matcher = boxofficePatt.matcher(boxofficeChar);
		System.out.println("Domestic - Foreign - Worldwide\n");
		while (matcher.find()) {
			System.out.println(matcher.group(1) + " - " + matcher.group(2)
					+ " - " + matcher.group(3));
		}
	}

	public void boxofficemojoSuche() throws IOException {
		boxofficeSearch = new URL(boxofficeSearchString + sucheFilm);
		boxofficeSucheChar = getURLContent(boxofficeSearch);
		Matcher matcher = boxofficeSuchePatt.matcher(boxofficeSucheChar);
		while (matcher.find()) {
			boxofficeTitleString = matcher.group(1);
		}
	}

	public void ffaListe() throws IOException {
		if (filminfoChar == null) {
			filminfoChar = getURLContent(filminfoURL);
		}
		Matcher m2;
		Matcher m1 = titlePatt.matcher(filminfoChar);
		while (m1.find()) {
			jahr = m1.group(2);
			Integer jahrInt = Integer.parseInt(jahr);
			System.out.println(jahrInt);
			jahrInt++;
			ffaURL = new URL(
					"http://www.ffa.de/start/content.phtml?page=filmhitlisten&language=&st=0&typ=14&jahr="
							+ jahr + "&submit2=GO&titelsuche=" + sucheFilm);
			ffaURLpuls1 = new URL(
					"http://www.ffa.de/start/content.phtml?page=filmhitlisten&language=&st=0&typ=14&jahr="
							+ jahrInt + "&submit2=GO&titelsuche=" + sucheFilm);
		}

		CharSequence ffaURLChar = getURLContent(ffaURL);
		CharSequence ffaURLpuls1Char = getURLContent(ffaURLpuls1);

		m1 = ffaZahlenePatt.matcher(ffaURLChar);
		m2 = ffaZahlenePatt.matcher(ffaURLpuls1Char);
		System.out.println("FFA Suchergebnisse:\n");
		while (m1.find()) {
			String title = m1.group(1);
			String titlenew = "";
			String[] result = title.split("<br\\s/>");
			for (int i = 0; i < result.length; i++) {
				titlenew = titlenew + " " + result[i];
			}
			String titlenewnew = "";
			result = titlenew.split("\n");
			for (int i = 0; i < result.length; i++) {
				titlenewnew = titlenewnew + " " + result[i];
			}

			System.out.println(jahr + ": " + titlenewnew + "Besucherzahlen: "
					+ m1.group(2));
		}
		System.out.println("FFA Suchergebnisse ein Jahr Später:\n");
		while (m2.find()) {
			String title = m2.group(1);
			String titlenew = "";
			String[] result = title.split("<br\\s/>");
			for (int i = 0; i < result.length; i++) {
				titlenew = titlenew + " " + result[i];
			}
			String titlenewnew = "";
			result = titlenew.split("\n");
			for (int i = 0; i < result.length; i++) {
				titlenewnew = titlenewnew + " " + result[i];
			}
			System.out.println(jahr + " Plus eins: " + titlenewnew
					+ "Besucherzahlen: " + m2.group(2));
		}
	}

	void actors(String actorID) throws IOException {
		URL actorURL = new URL("http://www.imdb.com/name/nm" + actorID + "/");
		URL actorURLMobile = new URL("http://m.imdb.com/name/nm" + actorID
				+ "/");
		// CharSequence actorChar = getURLContent(actorURL);
		System.out.println(System.currentTimeMillis());
		CharSequence actorMobileChar = getURLContent(actorURLMobile);
		System.out.println(System.currentTimeMillis());
		Matcher actor = actorPictureMobilePatt.matcher(actorMobileChar);
		while (actor.find()) {
			System.out.println(actor.group(1));
			String bildURLString = actor.group(1);
			if (bildURLString.charAt(0) != 'h')
				break;
			URL bildURL = new URL(bildURLString);
			InputStream in = bildURL.openStream();
			byte[] buffer = new byte[8192];
			if (titlePlusJahr != null) {
				File folder = new File("D:\\" + titleString + File.separator);
				folder.mkdir();
				FileOutputStream out = new FileOutputStream(new File("D:\\"
						+ titleString + File.separator + actorID + ".jpg"));
				int _tmp = 0;
				while ((_tmp = in.read(buffer)) > 0) {
					out.write(buffer, 0, _tmp);
				}
				out.close();
			} else {
				File folder = new File("D:\\" + "Unkown" + File.separator);
				folder.mkdir();
				FileOutputStream out = new FileOutputStream(new File("D:\\"
						+ "Unkown" + File.separator + "actorID" + ".jpg"));
				int _tmp = 0;
				while ((_tmp = in.read(buffer)) > 0) {
					out.write(buffer, 0, _tmp);
				}
				out.close();
			}

		}
	}

	public static CharSequence getURLContent(URL url) throws IOException {
		URLConnection conn = url.openConnection();

		if (conn instanceof URLConnection) {
			conn.setRequestProperty(
					"User-Agent",
					"Mozilla/5.0 (X11; U; Linux i686; en-US; rv:1.9.0.1) Gecko/2008072820 Firefox/3.0.1");
		}

		String encoding = conn.getContentEncoding();
		if (encoding == null) {
			encoding = "ISO-8859-1";
		}
		BufferedReader br = new BufferedReader(new InputStreamReader(
				conn.getInputStream()));
		StringBuilder sb = new StringBuilder(16384);
		try {
			String line;
			while ((line = br.readLine()) != null) {

				sb.append(line);
				sb.append('\n');
			}
		} finally {
			br.close();
		}
		return sb;
	}

	public static void main(String[] args) throws IOException {
		HTMLRegex test = new HTMLRegex();
	}

}
