namespace TextBox

module Domain =

    open Types

    let fontSizeInPixels fontSize =
        match fontSize with
        | Px i -> i

    let insertAt (col, row) (ch:char) text =
        text |>
        List.mapi (fun i (str:string) ->
            if row = i then
                str.[0..col-1] + (string ch) + str.[col..]
            else
                str
        )

    let removeAt (col, row) text =
        text |>
        List.mapi (fun i (str:string) ->
            if row = i then
                str.[0..col-1] + str.[col+1..]
            else
                str
        )

    let insertRowAt row str (text:string list)=
        List.concat [
            text.[0..row - 1]
            [str]
            text.[row..]
        ]

    let replaceRowAt row replacement (text:string list)=
        List.mapi (fun i str ->
            if row = i then replacement else str
        ) text

    let removeRowAt row (text:string list)=
        text.[0..row-1]@text.[row+1..]

    let constrainCursor model (col, row) =
        let str row =
            if row >= 0 && row < List.length model.Text then model.Text.[row]
            else ""
        let newRow = min (max row 0) (List.length model.Text - 1)
        let newCol = min (max col 0) (String.length (str newRow))
        (newCol, newRow)

    let ensureVisible (model: TbState) =
        let fontSize = float (model.FontSizeInPixels ())
        let col, row = model.CursorPosition
        let scrollX, scrollY = model.Scroll
        let cursorX, cursorY = 0, (float row) * fontSize
        let bottom = (float scrollY) + 750.
        if cursorY > (bottom-fontSize) then
            { model with Scroll = (scrollX, (int cursorY) - 750 + (int fontSize)) }
        else if (int cursorY) < scrollY then
            { model with Scroll = (scrollX, (int cursorY)) }
        else
            model

    let update : Update = fun event model ->
        let curCol, curRow = model.CursorPosition
        let constrain = constrainCursor model
        match event with
        | MoveLeft ->
            { model with CursorPosition = constrain (curCol - 1, curRow) }
        | MoveRight ->
            { model with CursorPosition = constrain (curCol + 1, curRow) }
        | MoveUp ->
            { model with CursorPosition = constrain (curCol, curRow - 1) }
            |> ensureVisible
        | MoveDown ->
            { model with CursorPosition = constrain (curCol, curRow + 1)}
            |> ensureVisible
        | Return ->
            let str = model.Text.[curRow]
            let splitStr = (str.[0..curCol-1], str.[curCol..])
            { model
                with CursorPosition = (0, curRow + 1);
                     Text =
                        model.Text
                        |> replaceRowAt (curRow) (fst splitStr)
                        |> insertRowAt (curRow + 1) (snd splitStr)
            }
            |> ensureVisible
        | Tab -> model
        | KeyPress ch ->
            { model with
                CursorPosition = (curCol + 1, curRow)
                Text = insertAt model.CursorPosition ch model.Text }
        | Backspace ->
            if curCol = 0 && curRow >= 1 then
                let curStr = model.Text.[curRow]
                let prevStr = model.Text.[curRow-1]
                { model with
                    CursorPosition = (String.length prevStr, curRow - 1)
                    Text =
                        model.Text
                        |> removeRowAt curRow
                        |> replaceRowAt (curRow - 1) (prevStr + curStr)
                }
            else if curCol = 0 then
                model
            else
                { model with
                    CursorPosition = (curCol - 1, curRow)
                    Text = removeAt (curCol - 1, curRow) model.Text }
