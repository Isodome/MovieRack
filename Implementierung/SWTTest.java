import java.awt.Checkbox;
import java.awt.Component;
import java.awt.GridBagLayout;
import java.util.EventObject;

import javax.swing.Icon;
import javax.swing.JButton;
import javax.swing.JLabel;
import javax.swing.JList;
import javax.swing.ListCellRenderer;

import org.eclipse.swt.SWT;
import org.eclipse.swt.events.ShellEvent;
import org.eclipse.swt.events.ShellListener;
import org.eclipse.swt.graphics.Image;
import org.eclipse.swt.layout.FillLayout;
import org.eclipse.swt.layout.GridData;
import org.eclipse.swt.layout.GridLayout;
import org.eclipse.swt.layout.RowLayout;
import org.eclipse.swt.widgets.Button;
import org.eclipse.swt.widgets.Combo;
import org.eclipse.swt.widgets.Display;
import org.eclipse.swt.widgets.Event;
import org.eclipse.swt.widgets.Group;
import org.eclipse.swt.widgets.List;
import org.eclipse.swt.widgets.Listener;
import org.eclipse.swt.widgets.MessageBox;
import org.eclipse.swt.widgets.Shell;
import org.eclipse.swt.widgets.Table;
import org.eclipse.swt.widgets.TableColumn;
import org.eclipse.swt.widgets.TableItem;
import org.eclipse.swt.widgets.Widget;

public class SWTTest implements Listener, ShellListener {
	Display display = new Display();
	// Shell shell = new Shell(display, SWT.NO_TRIM | SWT.ON_TOP);
	// Shell shell = new Shell(display, SWT.SHELL_TRIM);
	Shell shell = new Shell(display);
/*
	Button clickbutton = new Button(shell, SWT.PUSH);
	Group toggleGroup = new Group(shell, SWT.SHADOW_IN);
	Button checkButton1 = new Button(shell, SWT.CHECK);
	Button flatButton = new Button(shell, SWT.FLAT);
	Button toggleButton1 = new Button(shell, SWT.TOGGLE);
	Button toggleButton2 = new Button(toggleGroup, SWT.TOGGLE);
	Button toggleButton3 = new Button(toggleGroup, SWT.TOGGLE);
	Button checkButton2 = new Button(shell, SWT.CHECK);
	Button arrowButton = new Button(shell, SWT.ARROW);
	Combo dogBreedCombo;
	List categories;

	public void test() {
		GridLayout gridLayout = new GridLayout();
		gridLayout.numColumns = 3;
		gridLayout.makeColumnsEqualWidth = false;

		Table table = new Table(shell, SWT.BORDER);
		table.setHeaderVisible(true);
		table.setLinesVisible(true);
		TableColumn column = new TableColumn(table, SWT.NONE);
		TableColumn column1 = new TableColumn(table, SWT.NONE);
		column.setText("Test");
		column1.setText("Test1");
		TableItem item = new TableItem(table, SWT.NONE);
		Image image = new Image(display, "bild/filesave.png");
		item.setImage(image);
		item.setText("Test");

		GridData gridData = new GridData();
		gridData.horizontalAlignment = GridData.FILL;
		gridData.verticalAlignment = GridData.FILL;
		gridData.grabExcessHorizontalSpace = true;
		gridData.grabExcessVerticalSpace = true;
		table.setLayoutData(gridData);

		GridData gridData1 = new GridData();
		gridData1.horizontalAlignment = GridData.CENTER;
		gridData1.verticalAlignment = GridData.CENTER;
		toggleButton1.setLayoutData(gridData1);

		GridData gridData2 = new GridData();
		gridData2.verticalAlignment = GridData.FILL;
		gridData2.grabExcessVerticalSpace = true;
		flatButton.setLayoutData(gridData2);

		dogBreedCombo = new Combo(shell, SWT.NONE);
		dogBreedCombo.setItems(new String[] { "Collie", "Pitbull", "Poodle",
				"Scottie", "Black Lab" });

		categories = new List(shell, SWT.MULTI | SWT.BORDER | SWT.V_SCROLL
				| SWT.H_SCROLL);
		categories.setItems(new String[] { "Best of Breed", "Prettiest Female",
				"Handsomest Male", "Best Dressed", "Fluffiest Ears",
				"Most Colors", "Best Performer", "Loudest Bark",
				"Best Behaved", "Prettiest Eyes", "Most Hair", "Longest Tail",
				"Cutest Trick" });

		shell.setLayout(gridLayout);
		shell.setText("Test");
		shell.addListener(SWT.Traverse, this);
		shell.addShellListener(this);
		Image bild = new Image(display, "bild/filesave.png");

		clickbutton.setImage(bild);
		clickbutton.setText("clickButton");
		checkButton1.setText("CheckButton1");
		checkButton2.setText("CheckButton2");
		checkButton1.addListener(SWT.Selection, this);
		toggleButton1.addListener(SWT.Selection, this);
		toggleGroup.setLayout(new RowLayout(SWT.VERTICAL));
		toggleGroup.setText("ToggleButtonGroup");
		toggleButton1.setText("ToggleButton1");
		toggleButton2.setText("ToggleButton2");
		toggleButton3.setText("ToggleButton3");

		arrowButton.setText("arrowButton");
		flatButton.setText("flatButton");
		// shell.setMaximized(true);
		shell.pack();
		shell.open();

		while (!shell.isDisposed()) {
			if (!display.readAndDispatch())
				display.sleep();
		}
		display.dispose();
	}
*/
	public void tabletest() {
		// final Display display = new Display();
		// Shell shell = new Shell(display);
		shell.setLayout(new FillLayout());
		shell.setText("Show results as a bar chart in Table");
		final Table table = new Table(shell, SWT.BORDER);
		//table.setHeaderVisible(true);
	//	table.setLinesVisible(true);
		TableColumn column1 = new TableColumn(table, SWT.NONE);
		column1.setText("Bug Status");
		column1.setWidth(30);
		final TableColumn column2 = new TableColumn(table, SWT.NONE);
		column2.setText("Percent");
		column2.setWidth(200);
		String[] labels = new String[] { "", "Title"};
		for (int i = 0; i < labels.length; i++) {
			TableItem item = new TableItem(table, SWT.NONE);
			item.setText(labels);
			Image image = new Image(display, "bild/filesave.png");
			item.setImage(image);
		}
		
		shell.pack();
		shell.open();
		while (!shell.isDisposed()) {
			if (!display.readAndDispatch())
				display.sleep();
		}
		display.dispose();
	}

	public static void main(String args[]) {
		SWTTest test = new SWTTest();
		test.tabletest();

	}

	public void jtreetest() {

	}
/*
	@Override
	public void handleEvent(Event event) {

		if (event.widget == checkButton1) {
			System.out.println("CHECK");
		} else if (event.widget == toggleButton1) {
			System.out.println("TOGGLE");
		}

		switch (event.detail) {
		case SWT.TRAVERSE_ESCAPE:
			shell.close();
			event.detail = SWT.TRAVERSE_NONE;
			event.doit = false;
			break;
		}

	}
*/
	@Override
	public void shellActivated(ShellEvent arg0) {
		// TODO Auto-generated method stub

	}

	@Override
	public void shellClosed(ShellEvent arg0) {
		// TODO Auto-generated method stub
		System.out.println("Beendet");
	}

	@Override
	public void shellDeactivated(ShellEvent arg0) {
		// TODO Auto-generated method stub
		shell.setText("pff");
	}

	@Override
	public void shellDeiconified(ShellEvent arg0) {
		// TODO Auto-generated method stub

	}

	@Override
	public void shellIconified(ShellEvent arg0) {
		// TODO Auto-generated method stub

	}

	class MyCellRenderer implements ListCellRenderer {

		public Component getListCellRendererComponent(JList list, Object value,
				int index, boolean selected, boolean focus) {
			return new JLabel("Hallo!");
		}
	}

	@Override
	public void handleEvent(Event arg0) {
		// TODO Auto-generated method stub
		
	}
}