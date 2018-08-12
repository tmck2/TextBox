module Tests

open Xunit

open TextBox.Types
open TextBox.Domain

let baseModel = {
    Font = "Arial", Px 30
    CursorPosition = (0,0)
    Scroll = 0, 0
    Text = [""]
}

[<Fact>]
let ``Adding a character in the middle`` () =
    let initial = { baseModel with CursorPosition=(1,0); Text=["Test"] }
    let expected = { baseModel with CursorPosition=(2,0); Text=["T1est"] }
    let actual = update (KeyPress '1') initial
    Assert.Equal(expected, actual)

[<Fact>]
let ``Adding a character to the end`` () =
    let initial = { baseModel with CursorPosition=(4,0); Text=["Test"] }
    let expected = { baseModel with CursorPosition=(5,0); Text=["Test1"] }
    let actual = update (KeyPress '1') initial
    Assert.Equal(expected, actual)

[<Fact>]
let ``Adding a character to the beginning`` () =
    let initial = { baseModel with CursorPosition=(0,0); Text=["Test"] }
    let expected = { baseModel with CursorPosition=(1,0); Text=["1Test"] }
    let actual = update (KeyPress '1') initial
    Assert.Equal(expected, actual)

[<Fact>]
let ``Remove character from the middle`` () =
    let initial = { baseModel with CursorPosition=(2,0); Text=["Test"] }
    let expected = { baseModel with CursorPosition=(1,0); Text=["Tst"] }
    let actual = update Backspace initial
    Assert.Equal(expected, actual)

[<Fact>]
let ``Remove character from the beginning`` () =
    let initial = { baseModel with CursorPosition=(0,0); Text=["Test"] }
    let expected = { baseModel with CursorPosition=(0,0); Text=["Test"] }
    let actual = update Backspace initial
    Assert.Equal(expected, actual)

[<Fact>]
let ``Remove character from the end`` () =
    let initial = { baseModel with CursorPosition=(4,0); Text=["Test"] }
    let expected = { baseModel with CursorPosition=(3,0); Text=["Tes"] }
    let actual = update Backspace initial
    Assert.Equal(expected, actual)

[<Fact>]
let ``Prevents moving right when cursor is at the end of the line`` () =
    let initial = { baseModel with CursorPosition=(4,0); Text=["Test"] }
    let expected = { baseModel with CursorPosition=(4,0); Text=["Test"] }
    let actual = update MoveRight initial
    Assert.Equal(expected, actual)

[<Fact>]
let ``Prevents moving left when cursor is at the beginning of the line`` () =
    let initial = { baseModel with CursorPosition=(0,0); Text=["Test"] }
    let expected = { baseModel with CursorPosition=(0,0); Text=["Test"] }
    let actual = update MoveLeft initial
    Assert.Equal(expected, actual)

[<Fact>]
let ``Prevents moving up when cursor is at the top`` () =
    let initial = { baseModel with CursorPosition=(0,0); Text=["Test"] }
    let expected = { baseModel with CursorPosition=(0,0); Text=["Test"] }
    let actual = update MoveUp initial
    Assert.Equal(expected, actual)

[<Fact>]
let ``Moving up moves to same column on previous row`` () =
    let initial = { baseModel with CursorPosition=(3,1); Text=["Line 1";"Line 2"] }
    let expected = { baseModel with CursorPosition=(3,0); Text=["Line 1";"Line 2"] }
    let actual = update MoveUp initial
    Assert.Equal(expected, actual)

[<Fact>]
let ``Moving up does not move further than the last character of the previous row`` () =
    let initial = { baseModel with CursorPosition=(6,1); Text=["Line";"Longer Line"] }
    let expected = { baseModel with CursorPosition=(4,0); Text=["Line";"Longer Line"] }
    let actual = update MoveUp initial
    Assert.Equal(expected, actual)

[<Fact>]
let ``Moving down moves to same column on next row`` () =
    let initial = { baseModel with CursorPosition=(4,0); Text=["Line";"Longer Line"] }
    let expected = { baseModel with CursorPosition=(4,1); Text=["Line";"Longer Line"] }
    let actual = update MoveDown initial
    Assert.Equal(expected, actual)

[<Fact>]
let ``Cannot move down past last row`` () =
    let initial = { baseModel with CursorPosition=(0,1); Text=["Line";"Longer Line"] }
    let expected = { baseModel with CursorPosition=(0,1); Text=["Line";"Longer Line"] }
    let actual = update MoveDown initial
    Assert.Equal(expected, actual)

[<Fact>]
let ``Pressing enter inserts a new row`` () =
    let initial = { baseModel with CursorPosition=(6,0); Text=["Line 1";"Line 2"] }
    let expected = { baseModel with CursorPosition=(0,1); Text=["Line 1";"";"Line 2"] }
    let actual = update Return initial
    Assert.Equal(expected, actual)

[<Fact>]
let ``Pressing enter in the middle of a row`` () =
    let initial = { baseModel with CursorPosition=(2,0); Text=["Line"] }
    let expected = { baseModel with CursorPosition=(0,1); Text=["Li";"ne"] }
    let actual = update Return initial
    Assert.Equal(expected, actual)

[<Fact>]
let ``Pressing backspace at the beginning of a line combines the rest of the string on the current line with the previous`` () =
    let initial = { baseModel with CursorPosition=(0,1); Text=["Line 1";"Line 2"] }
    let expected = { baseModel with CursorPosition=(6,0); Text=["Line 1Line 2"] }
    let actual = update Backspace initial
    Assert.Equal(expected, actual)
