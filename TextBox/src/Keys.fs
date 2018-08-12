namespace TextBox

module Keys =

    type Keys =
        | BACKSPACE = 8
        | RETURN = 13
        | LEFT = 37
        | UP = 38
        | RIGHT = 39
        | DOWN = 40

    let keyFromKeyCode keyCode =
        match keyCode with
        | 8. -> Some Keys.BACKSPACE
        | 13. -> Some Keys.RETURN
        | 37. -> Some Keys.LEFT
        | 38. -> Some Keys.UP
        | 39. -> Some Keys.RIGHT
        | 40. -> Some Keys.DOWN
        | _ -> None
