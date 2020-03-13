import React, { useState } from 'react';
import logo from './logo.svg';
import './App.css';
import ListUser from './listuser';
import Facereconition from './faceapireconition';
import Camaraupload from './uploadcamara'
function App() {
  const [component, setComponet] = useState(1);
  let showcomponent=null;

  if (component===1) {
    showcomponent = <Camaraupload/>;
  } else if(component===2) {
    showcomponent = <Facereconition/>;
  } else{
    showcomponent = <ListUser/>;
  }
  return (
    <div className="App">
      <header className="App-header">
        <img src={logo} className="App-logo" alt="logo" />
        <p>
          Face Api Reconition
        </p>
        <a
          className="App-link"
          href="https://reactjs.org"
          target="_blank"
          rel="noopener noreferrer"
        >
        </a>
      </header>
      <div>
      <button onClick={() => setComponet(1)}>Take Picture</button>
      <button onClick={() => setComponet(2)}>Reconition</button>
      <button onClick={() => setComponet(3)}>List user</button>
        
        {showcomponent}
      </div>
    </div>
  );
}

export default App;
