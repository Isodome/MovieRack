import java.io.File;
import java.io.FileNotFoundException;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.OutputStream;
import java.util.Properties;

import javax.xml.parsers.DocumentBuilder;
import javax.xml.parsers.DocumentBuilderFactory;
import org.w3c.dom.Document;
import org.w3c.dom.Element;
import org.w3c.dom.Node;
import org.w3c.dom.NodeList;

public class XMLParser {

	public static void test() throws IOException {
		Properties props = new Properties();
		props.setProperty("email.support", "donot-spam-me@nospam.com");
		props.setProperty("Test123", "Pfad");
		// where to store?
		OutputStream os = new FileOutputStream("test.xml");

		// store the properties detail into a pre-defined XML file
		props.storeToXML(os, "Support Email", "UTF-8");
		System.out.println("Done");
	}

	public static void main(String argv[]) throws IOException {
		test();
		
		try {
			File dir1 = new File(".");
			File dir2 = new File("..");
			System.out.println(dir1.getAbsolutePath());
			File file = new File("Config.xml");
			DocumentBuilderFactory dbf = DocumentBuilderFactory.newInstance();
			DocumentBuilder db = dbf.newDocumentBuilder();
			Document doc = db.parse(file);
			doc.getDocumentElement().normalize();

			System.out.println("Root element "
					+ doc.getDocumentElement().getNodeName());

			NodeList nodeLst = doc.getElementsByTagName("employee");

			System.out.println("Information of all employees");

			for (int s = 0; s < nodeLst.getLength(); s++) {
				Node fstNode = nodeLst.item(s);
				if (fstNode.getNodeType() == Node.ELEMENT_NODE) {

					Element fstElmnt = (Element) fstNode;
					NodeList fstNmElmntLst = fstElmnt
							.getElementsByTagName("firstname");
					Element fstNmElmnt = (Element) fstNmElmntLst.item(0);
					NodeList fstNm = fstNmElmnt.getChildNodes();
					System.out.println("First Name : "
							+ ((Node) fstNm.item(0)).getNodeValue());

					NodeList lstNmElmntLst = fstElmnt
							.getElementsByTagName("lastname");
					Element lstNmElmnt = (Element) lstNmElmntLst.item(0);
					NodeList lstNm = lstNmElmnt.getChildNodes();
					System.out.println("Last Name : "
							+ ((Node) lstNm.item(0)).getNodeValue());
				}

			}
		} catch (Exception e) {
			e.printStackTrace();
		}
	}
}