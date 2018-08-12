# Textbox POC

Little textbox proof of concept used to mess around with F# / FABLE.  Draws a multiline text box and allows editing of the README.md file which is loaded via an async request.

TODO:
* Add size to model and remove hard-coded size
* Move by word
* Home/End
* Modify ensureVisible to scroll left/right
* Add selection and copy/cut/paste
* Repeat keys that are held down

IDEAS:
* Syntax highlighting
* Add VIM mode
* Load and save files (maybe from dropbox, google drive, etc.)

## Building and running the app

> In the commands below, yarn is the tool of choice. If you want to use npm, just replace `yarn` by `npm` in the commands.

* Install dependencies: `yarn`
* Start Fable daemon and [Webpack](https://webpack.js.org/) dev server: `yarn start`
* In your browser, open: http://localhost:8080/

> Check the `scripts` section in `package.json` for more info. If you are using VS Code + [Ionide](http://ionide.io/), you can also use F5 or Ctrl+Shift+B (Cmd+Shift+B on macOS) instead of typing `yarn start`. With this Fable-specific errors will be highlighted in the editor along with other F# errors. See _Debugging in VS Code_ below.

Any modification you do to the F# code will be reflected in the web page after saving. When you want to output the JS code to disk, run `yarn build` and you'll get your frontend files ready for deployment in the `build` folder.

## Debugging in VS Code

* Install [Debugger For Chrome](https://marketplace.visualstudio.com/items?itemName=msjsdiag.debugger-for-chrome) in vscode
* Press F5 in vscode
* After all the .fs files are compiled, the browser will be launched
* Set a breakpoint in F#
* Restart with Ctrl+Shift+F5 (Cmd+Shift+F5 on macOS)
* The breakpoint will be caught in vscode