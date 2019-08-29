'samples from: https://www.musicradar.com/news/sampleradar-494-free-essential-drum-kit-samples

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
        ' path = Application.UserAppDataPath & "\Liasm\" 'TODO
        path = "C:\console_beat\" + id.ToString + ".wav"

        path = Chr(34) & path & Chr(34)

        ' MsgBox(path)
        Return path

    End Function
End Class

Module Module1

    Dim sound As New Sound
    Dim intsound As Double = 0
    Dim temposleep As Integer
    Dim beat = 8

    Dim hand1(16) As Boolean
    Dim hand2(16) As Boolean

    Dim kick_counter = 0 '0-3
    Dim snare_counter = 0 '100-103
    Dim hh_c_counter = 0 '200-203
    Dim hh_o_counter = 0 '300-303
    Dim cin1_counter = 0 '400-403
    Dim cin2_counter = 0 '500-503
    Dim tom1_counter = 0 '600-603
    Dim tom2_counter = 0 '700-703
    Dim tom3_counter = 0 '800-803

    Dim kick_pattern(16) As Boolean
    Dim snare_pattern(16) As Boolean
    Dim hh_c_pattern(16) As Boolean
    Dim hh_o_pattern(16) As Boolean
    Dim cin1_pattern(16) As Boolean
    Dim cin2_pattern(16) As Boolean
    Dim tom1_pattern(16) As Boolean
    Dim tom2_pattern(16) As Boolean
    Dim tom3_pattern(16) As Boolean

    Dim sound_assigned(9) As Integer
    Dim pattern_count As Integer
    Dim repeat As Integer
    Dim first_rodeo = True
    Dim minsleep = 0
    Dim maxsleep = 300
    Dim acceleration_acc
    Dim acceleration_type
    Dim acceleration_amount

    Sub Main()

        Dim i As Byte
re:
        i = 0
        generate_pattern()
        display_tab()
        Do
            i += 1
            play_pattern()
            If i = repeat Then
                GoTo re
            End If
        Loop

    End Sub

    Private Sub generate_pattern()
        Console.Clear()
        Console.WriteLine("--- generating pattern ---")
        Console.WriteLine("")
        Dim r As Integer
        Dim beat_stay = False

        'pattern change
        Randomize()
        r = Int((2 * Rnd()))    ' 0-1
        If r = 0 Or first_rodeo = True Then
            Console.WriteLine("pattern changes")
            beat_stay = False
            empty_all_pattern()
        Else
            beat_stay = True
            pattern_count = 0
            Console.WriteLine("pattern stays")
        End If

        'kit change
        Randomize()
        r = Int((2 * Rnd()))    ' 0-1
        If r = 0 Or first_rodeo = True Then
            assign_samples() 'kit change
        Else
            Console.WriteLine("samples stay")
        End If



        'Tempo change
        Randomize()
        r = Int((2 * Rnd()))    ' 0-1
        If r = 0 Or first_rodeo = True Then
            Console.WriteLine("tempo changes")
            Randomize()
            r = Int((275 * Rnd()) + 25)    ' 0-1
            temposleep = r
        Else
            Console.WriteLine("tempo stays")
        End If
        'temposleep = 800 'override
        Console.WriteLine("")
        Console.WriteLine("sleep=" + temposleep.ToString)

        ''acceleration
        'Randomize()
        'r = Int((2 * Rnd()))
        'If r = 0 And first_rodeo = False Then 'yes we'll do acceleration
        '    Randomize()
        '    r = Int((3 * Rnd()))
        '    If r = 0 Then 'every beat
        '        acceleration_type = 1
        '        Console.WriteLine("acceleration_type=each beat")
        '    ElseIf r = 1 Then 'every second beat
        '        acceleration_type = 2
        '        Console.WriteLine("acceleration_type=every second beat")
        '    Else 'from halfbeat
        '        acceleration_type = 3
        '        Console.WriteLine("acceleration_type=from halfbeat")
        '    End If

        '    'amount
        '    Randomize()
        '    acceleration_amount = 2 'Int((15 * Rnd()) + 15) ' 15-30

        '    'acc or dec
        '    Randomize()
        '    r = Int((2 * Rnd())) ' 0-2
        '    If r = 0 Then 'decc
        '        acceleration_acc = 0
        '        'acceleration_amount -= (acceleration_amount * 2)
        '    Else 'acc
        '        acceleration_acc = 1
        '    End If
        'Else
        '    acceleration_type = 0
        '    acceleration_amount = 0
        '    acceleration_acc = 0
        '    Console.WriteLine("acceleration_type=0")
        'End If
        'Console.WriteLine("acceleration_amount=" + acceleration_amount.ToString)

        'If acceleration_acc = 0 Then
        '    Console.WriteLine("acceleration_direction=" + acceleration_acc.ToString)
        'Else
        '    Console.WriteLine("acceleration_direction=" + acceleration_acc.ToString)
        'End If


        'kits
        Dim s As String = "samples=("
        For i = 0 To 8
            s += sound_assigned(i).ToString
            If i < 8 Then
                s += ", "
            End If

        Next
        s += ")"
        ' Console.WriteLine(s)

        If (beat_stay = True) Then
            Console.WriteLine("beat=" + beat.ToString)
            Console.WriteLine("repeat=" + repeat.ToString)
        Else
            'beat change
            Randomize()
            r = Int((2 * Rnd()))    ' 0-1
            If r = 0 Then
                beat = 4
                repeat = 8
            Else
                beat = 8
                repeat = 4
            End If
            'double repeat
            Randomize()
            r = Int((2 * Rnd()))    ' 0-1
            If r = 0 Then
                repeat *= 2
            End If
            Console.WriteLine("beat=" + beat.ToString)
            Console.WriteLine("repeat=" + repeat.ToString)


            'metronome hh or random
            Dim metronome_hh As Boolean
            'Randomize()
            ' r = Int((2 * Rnd()))    ' 0-1

            If 1 = 1 Then 'every second hh
                'Console.WriteLine("metronome=true")
                metronome_hh = True
                Dim Second As Boolean = True
                For i As Integer = 0 To beat
                    If Second = True Then
                        hh_c_pattern(i) = True
                        Second = False
                    Else
                        Second = True
                    End If
                Next
            Else
                Console.WriteLine("metronome=false")
                metronome_hh = False
            End If

            For i As Integer = 0 To beat - 1

                'KICK
                Randomize()
                r = Int((2 * Rnd()))    ' 0-1
                If (r = 1) Then
                    kick_pattern(i) = True
                Else
                    kick_pattern(i) = False
                End If

                'SNARE
                If hand1(i) = False Or hand2(i) = False Then
                    Randomize()
                    r = Int((4 * Rnd()))    ' 0-3
                    If (r = 0) Then
                        snare_pattern(i) = True
                        If hand1(i) = False Then
                            hand1(i) = True
                        Else
                            hand2(i) = True
                        End If
                    Else
                        snare_pattern(i) = False
                    End If
                End If

                'hh c - no metronome hh
                If metronome_hh = False Then
                    If hand1(i) = False Or hand2(i) = False Then
                        Randomize()
                        r = Int((4 * Rnd()))    ' 0-3
                        If (r = 0) Then
                            hh_c_pattern(i) = True
                            If hand1(i) = False Then
                                hand1(i) = True
                            Else
                                hand2(i) = True
                            End If
                        Else
                            hh_c_pattern(i) = False
                        End If
                    End If
                End If

                'hh o
                If hand1(i) = False Or hand2(i) = False Then
                    Randomize()
                    r = Int((4 * Rnd()))    ' 0-3
                    If r = 0 Then
                        hh_o_pattern(i) = True
                        If hand1(i) = False Then
                            hand1(i) = True
                        Else
                            hand2(i) = True
                        End If
                    Else
                        hh_o_pattern(i) = False
                    End If
                End If

                'Cin1
                If hand1(i) = False Or hand2(i) = False Then
                    Randomize()
                    r = Int((8 * Rnd()))    ' 0-7
                    If (r = 0) Then
                        cin1_pattern(i) = True
                        If hand1(i) = False Then
                            hand1(i) = True
                        Else
                            hand2(i) = True
                        End If
                    Else
                        cin1_pattern(i) = False
                    End If
                End If

                'Cin2
                If hand1(i) = False Or hand2(i) = False Then
                    Randomize()
                    r = Int((8 * Rnd()))    ' 0-7
                    If (r = 0) Then
                        cin2_pattern(i) = True
                        If hand1(i) = False Then
                            hand1(i) = True
                        Else
                            hand2(i) = True
                        End If
                    Else
                        cin2_pattern(i) = False
                    End If
                End If

                'tom1
                If hand1(i) = False Or hand2(i) = False Then
                    Randomize()
                    r = Int((8 * Rnd()))    ' 0-7
                    If (r = 0) Then
                        tom1_pattern(i) = True
                        If hand1(i) = False Then
                            hand1(i) = True
                        Else
                            hand2(i) = True
                        End If
                    Else
                        tom1_pattern(i) = False
                    End If
                End If

                'tom2
                If hand1(i) = False Or hand2(i) = False Then
                    Randomize()
                    r = Int((8 * Rnd()))    ' 0-7
                    If (r = 0) Then
                        tom2_pattern(i) = True
                        If hand1(i) = False Then
                            hand1(i) = True
                        Else
                            hand2(i) = True
                        End If
                    Else
                        tom2_pattern(i) = False
                    End If
                End If

                'tom3
                If hand1(i) = False Or hand2(i) = False Then
                    Randomize()
                    r = Int((8 * Rnd()))    ' 0-7
                    If (r = 0) Then
                        tom3_pattern(i) = True
                        If hand1(i) = False Then
                            hand1(i) = True
                        Else
                            hand2(i) = True
                        End If
                    Else
                        tom3_pattern(i) = False
                    End If
                End If

            Next

        End If
        Console.WriteLine("")
        first_rodeo = False

    End Sub

    Private Sub display_tab()
        Dim s As String

        s = "cin1" & vbTab + sound_assigned(4).ToString & vbTab
        For i = 0 To beat - 1
            If cin1_pattern(i) = True Then
                s += "1 "
            Else
                s += "0 "
            End If
        Next
        Console.WriteLine(s)

        s = "cin2" & vbTab + sound_assigned(5).ToString & vbTab
        For i = 0 To beat - 1
            If cin2_pattern(i) = True Then
                s += "1 "
            Else
                s += "0 "
            End If
        Next
        Console.WriteLine(s)

        s = "hihat1" & vbTab & sound_assigned(2).ToString & vbTab
        For i = 0 To beat - 1
            If hh_c_pattern(i) = True And hh_o_pattern(i) = False Then
                s += "1 "
            Else
                s += "0 "
            End If
        Next
        Console.WriteLine(s)

        s = "hihat2" & vbTab & sound_assigned(3).ToString & vbTab
        For i = 0 To beat - 1
            If hh_o_pattern(i) = True Then
                s += "1 "
            Else
                s += "0 "
            End If
        Next
        Console.WriteLine(s)

        s = "snare" & vbTab & sound_assigned(1).ToString & vbTab
        For i = 0 To beat - 1
            If snare_pattern(i) = True Then
                s += "1 "
            Else
                s += "0 "
            End If
        Next
        Console.WriteLine(s)

        s = "tom1" & vbTab & sound_assigned(6).ToString & vbTab
        For i = 0 To beat - 1
            If tom1_pattern(i) = True Then
                s += "1 "
            Else
                s += "0 "
            End If
        Next
        Console.WriteLine(s)

        s = "tom2" & vbTab & sound_assigned(7).ToString & vbTab
        For i = 0 To beat - 1
            If tom2_pattern(i) = True Then
                s += "1 "
            Else
                s += "0 "
            End If
        Next
        Console.WriteLine(s)

        s = "tom3" & vbTab & sound_assigned(8).ToString & vbTab
        For i = 0 To beat - 1
            If tom3_pattern(i) = True Then
                s += "1 "
            Else
                s += "0 "
            End If
        Next
        Console.WriteLine(s)

        s = "kick" & vbTab & sound_assigned(0).ToString & vbTab
        For i = 0 To beat - 1
            If kick_pattern(i) = True Then
                s += "1 "
            Else
                s += "0 "
            End If
        Next
        Console.WriteLine(s)

        Console.WriteLine("")
    End Sub

    'Private Sub apply_acceleration()

    '    If acceleration_acc = 0 Then 'dec
    '        If temposleep / acceleration_amount < minsleep And temposleep / acceleration_amount > maxsleep Then
    '            temposleep /= acceleration_amount
    '        Else
    '            Console.Write("cant apply dec")
    '        End If
    '    Else 'acc
    '        If temposleep * acceleration_amount < minsleep And temposleep * acceleration_amount > maxsleep Then
    '            temposleep *= acceleration_amount
    '        Else
    '            Console.Write("cant apply acc")
    '        End If
    '    End If
    'End Sub

    Private Sub play_pattern()

        pattern_count += 1
        ' Console.Write(pattern_count.ToString) '"playing loop: " +
        Console.Write(".")

        'Select Case acceleration_type

        '    Case 1 'every beat
        '        apply_acceleration()
        '    Case 2 'every second beat
        '        If pattern_count = 2 Or pattern_count = 4 Or pattern_count = 6 Or pattern_count = 8 Then 'todo...
        '            apply_acceleration()
        '        End If
        '    Case 3 'from halfbeat
        '        If pattern_count - 1 = beat / 2 Then
        '            apply_acceleration()
        '        End If

        'End Select



        For i As Integer = 0 To beat - 1

            ' Console.WriteLine((i + 1).ToString)

            If kick_pattern(i) = True Then
                play_kick()
            End If

            If hh_o_pattern(i) = True Then
                play_hh_o()
            ElseIf hh_c_pattern(i) = True Then
                play_hh_c()
            End If

            If snare_pattern(i) = True Then
                play_snare()
            End If

            If cin1_pattern(i) = True Then
                play_cin1()
            End If

            If cin2_pattern(i) = True Then
                play_cin2()
            End If

            If tom1_pattern(i) = True Then
                play_tom1()
            End If

            If tom2_pattern(i) = True Then
                play_tom2()
            End If

            If tom3_pattern(i) = True Then
                play_tom3()
            End If

            Threading.Thread.Sleep(temposleep)
        Next

    End Sub

    Private Sub empty_all_pattern()

        pattern_count = 0
        For i As Integer = 0 To beat - 1
            hand1(i) = False
            hand2(i) = False
            hh_o_pattern(i) = False
            hh_c_pattern(i) = False
            kick_pattern(i) = False
            snare_pattern(i) = False
            cin1_pattern(i) = False
            cin2_pattern(i) = False
            tom1_pattern(i) = False
            tom2_pattern(i) = False
            tom3_pattern(i) = False
        Next

    End Sub

    Private Sub assign_samples()
        Console.WriteLine("samples change")
        Dim r

        For i As Integer = 0 To 8

            Randomize()
            r = Int((504 * Rnd()))    ' 0-503
            sound_assigned(i) = r

            'quarantine
            If (r >= 63 And r <= 70) Or (r >= 435 And r <= 439) Or r = 83 Or r = 270 Or r = 455 Or r = 102 Or r = 103 Then
                i -= 1
            End If

        Next

    End Sub

    Private Sub HaltSound()
        For i = 0 To 1000 'intsound
            sound.kill("SOUND" & i)
        Next
        intsound = 0
    End Sub

    Private Sub play_kick()
        'Console.WriteLine("kick")
        If kick_counter < 3 Then
            kick_counter += 1
            sound.kill("SOUND" & kick_counter)
        Else
            kick_counter = 0
            sound.kill("SOUND" & kick_counter)
        End If
        With sound
            .Name = "SOUND" & kick_counter
            '.Play((0), False)
            .Play(sound_assigned(0), False)
        End With
    End Sub

    Private Sub play_snare()
        'Console.WriteLine("snare")
        If snare_counter < 3 Then
            snare_counter += 1
            sound.kill("SOUND" & snare_counter + 100)
        Else
            snare_counter = 0
            sound.kill("SOUND" & snare_counter + 100)
        End If
        With sound
            .Name = "SOUND" & snare_counter + 100
            ' .Play((1), False)
            .Play((sound_assigned(1)), False)
        End With
    End Sub

    Private Sub play_hh_c()
        'Console.WriteLine("hihat closed")

        If hh_c_counter < 3 Then
            hh_c_counter += 1
            sound.kill("SOUND" & hh_c_counter + 200)
        Else
            hh_c_counter = 0
            sound.kill("SOUND" & hh_c_counter + 200)
        End If

        With sound
            .Name = "SOUND" & hh_c_counter + 200
            '.Play((2), False)
            .Play((sound_assigned(2)), False)
        End With
    End Sub

    Private Sub play_hh_o()
        'Console.WriteLine("hihat open")

        If hh_o_counter < 3 Then
            hh_o_counter += 1
            sound.kill("SOUND" & hh_o_counter + 300)
        Else
            hh_o_counter = 0
            sound.kill("SOUND" & hh_o_counter + 300)
        End If

        With sound
            .Name = "SOUND" & hh_o_counter + 300
            '.Play((3), False)
            .Play((sound_assigned(3)), False)
        End With
    End Sub

    Private Sub play_cin1()
        ' Console.WriteLine("cin1")
        If cin1_counter < 3 Then
            cin1_counter += 1
            sound.kill("SOUND" & cin1_counter + 400)
        Else
            cin1_counter = 0
            sound.kill("SOUND" & cin1_counter + 400)
        End If

        With sound
            .Name = "SOUND" & cin1_counter + 400
            '.Play((4), False)
            .Play((sound_assigned(4)), False)
        End With
    End Sub

    Private Sub play_cin2()
        ' Console.WriteLine("cin2")
        If cin2_counter < 3 Then
            cin2_counter += 1
            sound.kill("SOUND" & cin2_counter + 500)
        Else
            cin2_counter = 0
            sound.kill("SOUND" & cin2_counter + 500)
        End If

        With sound
            .Name = "SOUND" & cin2_counter + 500
            '.Play((5), False)
            .Play((sound_assigned(5)), False)
        End With
    End Sub

    Private Sub play_tom1()
        '  Console.WriteLine("tom1")
        If tom1_counter < 3 Then
            tom1_counter += 1
            sound.kill("SOUND" & tom1_counter + 600)
        Else
            tom1_counter = 0
            sound.kill("SOUND" & tom1_counter + 600)
        End If

        With sound
            .Name = "SOUND" & tom1_counter + 600
            '.Play((6), False)
            .Play((sound_assigned(6)), False)
        End With
    End Sub

    Private Sub play_tom2()
        '  Console.WriteLine("tom2")
        If tom2_counter < 3 Then
            tom2_counter += 1
            sound.kill("SOUND" & tom2_counter + 700)
        Else
            tom2_counter = 0
            sound.kill("SOUND" & tom2_counter + 700)
        End If

        With sound
            .Name = "SOUND" & tom2_counter + 700
            '.Play((7), False)
            .Play((sound_assigned(7)), False)
        End With
    End Sub

    Private Sub play_tom3()
        ' Console.WriteLine("tom3")
        If tom3_counter < 3 Then
            tom3_counter += 1
            sound.kill("SOUND" & tom3_counter + 800)
        Else
            tom3_counter = 0
            sound.kill("SOUND" & tom3_counter + 800)
        End If

        With sound
            .Name = "SOUND" & tom3_counter + 800
            ' .Play((8), False)
            .Play((sound_assigned(8)), False)
        End With
    End Sub



End Module
