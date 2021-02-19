
Imports System.Data
Imports System.Data.SQLite
Imports System.Runtime.InteropServices

'
' Created by SharpDevelop.
' User: Horacio
' Date: 18/02/2021
' Time: 03:19 p. m.
' 
' To change this template use Tools | Options | Coding | Edit Standard Headers.
'
Partial Public Class MainForm
    Public Sub New()
        ' The Me.InitializeComponent call is required for Windows Forms designer support.
        Me.InitializeComponent()

        '
        ' TODO : Add constructor code after InitializeComponents
        '
    End Sub

    Dim DB_Path As String
    '' Tabla de instructores
    Dim TableInstructores As String = "Instructores"
    Dim ClaveCurso,
    NombreCurso,
    NombreInstructor,
    RFC,
    FechaExamen,
    Vigencia,
    Telefone,
    Compania,
    Calificacion As String

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Dim SQLiteCon As New SQLiteConnection(DB_Path)

        Try
            SQLiteCon.Open()
        Catch ex As Exception
            SQLiteCon.Dispose()
            SQLiteCon = Nothing
            MsgBox(ex.Message)
            Exit Sub
        End Try

        Dim TableDB As New DataTable

        Try
            LoadDB("select * from " & TableInstructores & " order by ClaveCurso", TableDB, SQLiteCon)
            DataGridView1.DataSource = Nothing
            DataGridView1.DataSource = TableDB
            DataGridView1.Columns("ClaveCurso").HeaderText = "ClaveCurso"
            DataGridView1.ClearSelection()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

        TableDB.Dispose()
        TableDB = Nothing
        SQLiteCon.Close()
        SQLiteCon.Dispose()
        SQLiteCon = Nothing
    End Sub


    '' ste es el boton para eliminar
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim SQLiteCon As New SQLiteConnection(DB_Path)

        Try
            SQLiteCon.Open()
        Catch ex As Exception
            SQLiteCon.Dispose()
            SQLiteCon = Nothing
            MsgBox(ex.Message)
            Exit Sub
        End Try

        If DataGridView1.RowCount = 0 Then
            MsgBox("No es posible, la tabla esta vacia", MsgBoxStyle.Critical, "Failed")
            Return
        End If

        If DataGridView1.SelectedRows.Count = 0 Then
            MsgBox("No es posibe eliminar , selecciona un registro", MsgBoxStyle.Critical, "Failed")
            Return
        End If

        If MsgBox("Eliminar registro ?", CType(MsgBoxStyle.Question + MsgBoxStyle.OkCancel, Global.Microsoft.VisualBasic.MsgBoxStyle), "Confirmation") = MsgBoxResult.Cancel Then Return

        Try
            If AllCellsSelected(DataGridView1) = True Then
                ExecuteNonQuery("delete from " & TableInstructores & "", SQLiteCon)
                SQLiteCon.Close()
                SQLiteCon.Dispose()
                SQLiteCon = Nothing

                Button3_Click(sender, e)
                Return
            End If

            For Each row As DataGridViewRow In DataGridView1.SelectedRows
                If row.Selected = True Then
                    ExecuteNonQuery("delete from " & TableInstructores & " where ClaveCurso='" & row.DataBoundItem(0).ToString & "'", SQLiteCon)
                End If
            Next
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

        SQLiteCon.Close()
        SQLiteCon.Dispose()
        SQLiteCon = Nothing

        Button4_Click(sender, e)
    End Sub

    '' Este es el método que usamos para actualizar.
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim SQLiteCon As New SQLiteConnection(DB_Path)

        Try
            SQLiteCon.Open()
        Catch ex As Exception
            SQLiteCon.Dispose()
            SQLiteCon = Nothing
            MsgBox(ex.Message)
            Exit Sub
        End Try

        Try
            ExecuteNonQuery("update " & TableInstructores & " set ClaveCurso='" & TextBox1.Text & "',NombreCurso='" & TextBox2.Text _
                            & "',NombreInstructor='" & TextBox3.Text & "',RFC='" & TextBox4.Text & "',Telefono='" & TextBox5.Text & "',Compania='" & TextBox6.Text & "',Calificacion='" & TextBox7.Text & "',FechaExamen='" & DateTimePicker1.Text & "',Vigencia='" & DateTimePicker2.Text & "' where ClaveCurso='" & TextBox1.Text & "'", SQLiteCon)
            MsgBox("Update successfully")
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

        SQLiteCon.Close()
        SQLiteCon.Dispose()
        SQLiteCon = Nothing

        Button4_Click(sender, e)

    End Sub


    '' Este es el método que usamos para guardar.
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim SQLiteCon As New SQLiteConnection(DB_Path)

        Try
            SQLiteCon.Open()
        Catch ex As Exception
            SQLiteCon.Dispose()
            SQLiteCon = Nothing
            MsgBox(ex.Message)
            Exit Sub
        End Try

        Try
            ExecuteNonQuery("insert into " & TableInstructores & " (ClaveCurso,NombreCurso,NombreInstructor,RFC,Calificacion,Telefono,Compania,FechaExamen,Vigencia) values ('" & TextBox1.Text & "','" & TextBox2.Text _
                            & "','" & TextBox3.Text & "','" & TextBox4.Text & "','" & TextBox5.Text & "','" & TextBox6.Text & "','" & TextBox7.Text & "','" & DateTimePicker1.Text & "','" & DateTimePicker2.Text & "')", SQLiteCon)
            MsgBox("Inserción exitosa")
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

        SQLiteCon.Close()
        SQLiteCon.Dispose()
        SQLiteCon = Nothing

        Button4_Click(sender, e)
    End Sub

    Private Sub DataGridView1_CellMouseDown(sender As Object, e As DataGridViewCellMouseEventArgs) Handles DataGridView1.CellMouseDown
        If AllCellsSelected(DataGridView1) = False Then
            If e.Button = MouseButtons.Right Then
                DataGridView1.CurrentCell = DataGridView1(e.ColumnIndex, e.RowIndex)
                Dim i As Integer
                With DataGridView1
                    If e.RowIndex >= 0 Then
                        i = .CurrentRow.Index
                        ClaveCurso = .Rows(i).Cells("ClaveCurso").Value.ToString
                        NombreCurso = .Rows(i).Cells("NombreCurso").Value.ToString
                        NombreInstructor = .Rows(i).Cells("NombreInstructor").Value.ToString
                        RFC = .Rows(i).Cells("RFC").Value.ToString
                        FechaExamen = .Rows(i).Cells("FechaExamen").Value.ToString
                        Vigencia = .Rows(i).Cells("Vigencia").Value.ToString
                        Compania = .Rows(i).Cells("Compania").Value.ToString
                        Calificacion = .Rows(i).Cells("Calificacion").Value.ToString
                        Telefone = .Rows(i).Cells("Telefono").Value.ToString

                    End If
                End With
            End If
        End If
    End Sub

    Private Sub DataGridView1_CellMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs)
        Dim i As Integer
        With DataGridView1
            If e.RowIndex >= 0 Then
                i = .CurrentRow.Index
                ClaveCurso = .Rows(i).Cells("ClaveCurso").Value.ToString
            End If
        End With
    End Sub

    '' Metodo inicial o bien de todo la vista.
    Sub MainFormLoad(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.CenterToScreen()
        Button2.Enabled = False

        DB_Path = "Data Source=" & Application.StartupPath & "\corpetro.db;"

        Dim SQLiteCon As New SQLiteConnection(DB_Path)
        Try
            SQLiteCon.Open()
        Catch ex As Exception
            SQLiteCon.Dispose()
            SQLiteCon = Nothing
            MsgBox(ex.Message)
            Exit Sub
        End Try

        Dim TableDB As New DataTable

        Try
            LoadDB("select*from " & TableInstructores & " order by ClaveCurso", TableDB, SQLiteCon)
            DataGridView1.DataSource = Nothing
            DataGridView1.DataSource = TableDB
            DataGridView1.Columns("ClaveCurso").HeaderText = "ClaveCurso"
            DataGridView1.ClearSelection()

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

        TableDB.Dispose()
        TableDB = Nothing
        SQLiteCon.Close()
        SQLiteCon.Dispose()
        SQLiteCon = Nothing
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        TextBox1.Clear()
        TextBox2.Clear()
        TextBox3.Clear()
        TextBox4.Clear()
        TextBox5.Clear()
        TextBox6.Clear()
        TextBox7.Clear()
        Button1.Enabled = True
        Button2.Enabled = False
    End Sub

    '' Método para seleccionar todas las celdas.
    Private Function AllCellsSelected(dgv As DataGridView) As Boolean
        AllCellsSelected = (DataGridView1.SelectedCells.Count = (DataGridView1.RowCount * DataGridView1.Columns.GetColumnCount(DataGridViewElementStates.Visible)))
    End Function

    '' Metodo para seleccionar
    Private Sub MainForm_MouseClick(sender As Object, e As MouseEventArgs) Handles Me.MouseClick
        DataGridView1.ClearSelection()
    End Sub


    Private Sub LoadDB(ByVal q As String, ByVal tbl As DataTable, ByVal cn As SQLiteConnection)
        Dim SQLiteDA As New SQLiteDataAdapter(q, cn)
        SQLiteDA.Fill(tbl)
        SQLiteDA.Dispose()
        SQLiteDA = Nothing
    End Sub

    'Sub to write to the database
    Private Sub ExecuteNonQuery(ByVal query As String, ByVal cn As SQLiteConnection)
        Dim SQLiteCM As New SQLiteCommand(query, cn)
        SQLiteCM.ExecuteNonQuery()
        SQLiteCM.Dispose()
        SQLiteCM = Nothing
    End Sub

    Private Sub ActualizarToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ActualizarToolStripMenuItem.Click
        If AllCellsSelected(DataGridView1) = False Then
            TextBox1.Text = ClaveCurso
            TextBox2.Text = NombreInstructor
            TextBox3.Text = RFC
            TextBox4.Text = Compania
            TextBox5.Text = Calificacion
            TextBox6.Text = NombreCurso
            TextBox7.Text = Telefone
            DateTimePicker1.Text = FechaExamen
            DateTimePicker2.Text = Vigencia

            Button1.Enabled = False
            Button2.Enabled = True
        Else
            MsgBox("Can not edit because table row is selected all. Please choose one to edit.", MsgBoxStyle.Critical, "Failed")
        End If
    End Sub

    Private Sub EliminarToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles EliminarToolStripMenuItem.Click
        Button3_Click(sender, e)
    End Sub

    Private Sub SeleccionarTodoToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SeleccionarTodoToolStripMenuItem.Click
        DataGridView1.SelectAll()
    End Sub

    Private Sub PERDIDOToolStripMenuItem_Click(sender As Object, e As EventArgs)
        If AllCellsSelected(DataGridView1) = False Then
            TextBox1.Text = ClaveCurso
            TextBox2.Text = NombreInstructor
            TextBox3.Text = RFC
            TextBox4.Text = Compania
            TextBox5.Text = Calificacion
            TextBox5.Text = NombreCurso
            TextBox7.Text = Telefone
            DateTimePicker1.Text = FechaExamen
            DateTimePicker2.Text = Vigencia

            Button1.Enabled = False
            Button2.Enabled = True
        Else
            MsgBox("Can not edit because table row is selected all. Please choose one to edit.", MsgBoxStyle.Critical, "Failed")
        End If
    End Sub
End Class
