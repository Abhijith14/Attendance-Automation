Imports System.Collections.ObjectModel
Imports System.IO
Imports System.Text
Imports ExcelDataReader
Imports OpenQA.Selenium
Imports OpenQA.Selenium.Chrome
Imports OpenQA.Selenium.Support.UI
Imports WebDriverManager
Imports WebDriverManager.DriverConfigs.Impl

Public Class Form1

    Dim tables As DataTableCollection


    'For Logging in
    Private Sub Login_fn(driver As IWebDriver, username_txt As String, Password_txt As String)

        driver.Navigate().GoToUrl("https://gcs.edunexttechnologies.com/")

        Dim username As IWebElement = driver.FindElement(By.XPath("//input[@id='username']"))
        username.SendKeys(username_txt)

        Dim password As IWebElement = driver.FindElement(By.XPath("//input[@id='password']"))
        password.SendKeys(Password_txt)

        System.Threading.Thread.Sleep(100)

        Dim loginbtn As IWebElement = driver.FindElement(By.XPath("//button[@id='user_login_btn']"))
        loginbtn.Click()

    End Sub

    ' Open Student Attendance

    Private Sub Open_student_attendance_fn(driver As IWebDriver)
        Dim stud_att As List(Of IWebElement) = New List(Of IWebElement)
        Dim cnt As Integer
        While True
            stud_att = driver.FindElements(By.XPath("//div[@class='module-box-name']")).ToList()
            cnt = stud_att.Count
            If cnt > 0 Then
                Exit While
            Else
                System.Threading.Thread.Sleep(100)
                Continue While
            End If
        End While

        For Each li As IWebElement In stud_att
            Try
                If li.Text.Equals("Student Attendance") Then
                    li.Click()
                End If

            Catch ex As OpenQA.Selenium.StaleElementReferenceException
                Console.WriteLine("Got one StaleElement")
                Continue For
            End Try
        Next
    End Sub


    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        Dim driver As IWebDriver

        driver = New ChromeDriver

        System.Threading.Thread.Sleep(5000)

        'Login_fn(driver, "GCS1221", "school1398")

        'Open_student_attendance_fn(driver)

        'Dim Sheet As New List(Of String)({"Sheet1", "Sheet2"})

        'For Each it In Sheet

        'Next

        ' get data from table



        Console.Read()

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Using ofd As OpenFileDialog = New OpenFileDialog() With {.Filter = "Excel Workbook|*.xlsx|Excel 97-2003 Workbook|*.xls"}
            If ofd.ShowDialog() = DialogResult.OK Then
                TextBox1.Text = ofd.FileName
                Using stream = File.Open(ofd.FileName, FileMode.Open, FileAccess.Read)
                    Using reader As IExcelDataReader = ExcelReaderFactory.CreateReader(stream)
                        Dim result As DataSet = reader.AsDataSet(New ExcelDataSetConfiguration() With {
                                                                 .ConfigureDataTable = Function(__) New ExcelDataTableConfiguration() With {
                                                                 .UseHeaderRow = True}})
                        tables = result.Tables
                        ComboBox1.Items.Clear()
                        For Each table As DataTable In tables
                            ComboBox1.Items.Add(table.TableName)
                        Next
                    End Using
                End Using
            End If
        End Using
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        Dim dt As DataTable = tables(ComboBox1.SelectedItem.ToString())
        DataGridView1.DataSource = dt
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim dt As DataTable = DataGridView1.DataSource

        'MessageBox.Show(dt.Rows.Count.ToString())

        'Console.WriteLine(dt(0, 0).value)
        For Each col_value In dt.Columns
            Console.WriteLine(dt(5))
        Next

        'For rCnt = 1 To dt.Rows.Count
        'For cCnt = 1 To dt.Columns.Count
        'Console.WriteLine(dt(rCnt, cCnt))
        'Next
        'Next

        'For Each col_values In dt.Columns
        ' For Each data_values In dt.Rows
        'Console.WriteLine(dt(col_values)(data_values))
        'Next
        'Next

    End Sub
End Class
