Imports Microsoft.VisualBasic

Public Class Sound

    Public _name As String = Nothing

    Public Declare Function mciSendString Lib "winmm.dll" Alias "mciSendStringA" (ByVal lpstrCommand As String, ByVal lpstrReturnString As String, ByVal uReturnLength As Integer, hwndCallback As Integer) As Integer


    Public Property Name() As String
        Get
            Return _name
        End Get
        Set(ByVal value As String)
            _name = value
        End Set
    End Property

    Public Sub Play(ByVal id As Integer, ByVal repeat As Boolean, Optional vol As Integer = 1000)
        Try
            mciSendString("Open " & getfile(id) & " alias " & _name, CStr(0), 0, 0)
            mciSendString("Play " & _name, CStr(0), 0, 0)
        Catch ex As Exception
            ' MsgBox(ex.ToString)
        End Try
    End Sub

    Public Sub kill(ByVal song As String)
        mciSendString("close " & song, CStr(0), 0, 0)
        _name = Nothing
    End Sub

    Private Function getfile(ByVal id As Integer) As String

        Dim path As String
        path = Application.UserAppDataPath & "\Liasm\"

        Select Case id
            'Case 0
                'path += "a1.wav"
            Case 1
                path += "a1.wav"
            Case 2
                path += "a2.wav"
            Case 3
                path += "a3.wav"
            Case 4
                path += "a4.wav"
            Case 5
                path += "a5.wav"
            Case 6
                path += "a6.wav"
            Case 7
                path += "a7.wav"
            Case 8
                path += "a8.wav"
            Case 9
                path += "a9.wav"
            Case 10
                path += "a10.wav"
            Case 11
                path += "a11.wav"
            Case 12
                path += "a12.wav"
            Case 13
                path += "a13.wav"
            Case 100
                path += "step1.wav"
            Case 101
                path += "step2.wav"
            Case 102
                path += "step3.wav"
            Case 103
                path += "step4.wav"
        End Select

        path = Chr(34) & path & Chr(34)

        ' MsgBox(path)

        Return path

    End Function
End Class