namespace TextBox

module Types =

    type Coordinate = int * int

    type FontFamily = string

    type FontSize =
        Px of int

    type Font = FontFamily * FontSize

    type TbState = {
        Font: Font
        CursorPosition: Coordinate
        Scroll: Coordinate
        Text: string list
    } with
        member x.FontString () =
            let (face, size) = x.Font
            let sizeStr =
                match size with
                | Px i -> sprintf "%ipx" i
            sprintf "%s %s" sizeStr face
        member x.FontSizeInPixels () =
            let (_, size) = x.Font
            match size with
            | Px i -> i

    type Event =
    | MoveLeft
    | MoveRight
    | MoveUp
    | MoveDown
    | Tab
    | KeyPress of char
    | Backspace
    | Return

    type Update = Event -> TbState -> TbState

    let fontSizeInPixels fontSize =
        match fontSize with
        | Px i -> i