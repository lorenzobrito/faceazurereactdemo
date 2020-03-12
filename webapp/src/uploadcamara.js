import React, { useState,Fragment, useRef , useEffect} from 'react';
import ReactDOM from 'react-dom';
import Camera from 'react-html5-camera-photo';
import 'react-html5-camera-photo/build/css/index.css';
import './App.css';
import axios from 'axios';
function Camaraupload() {
  const apiUrl = "https://localhost:5001/faceapi";
  const [username, setName] = useState('');
  
    function handleTakePhoto (dataUri) {
        axios.post(apiUrl, {  dataUri: dataUri,
        name : username })
          .then(res => {
            console.log(res);
            console.log(res.data);
          })
      }
  
      const handleInputChange = (e) => setName( e.currentTarget.value);
      return (
        <div>
            <div>
        <Camera
          onTakePhoto = {handleInputChange}
        />
        </div>
        <input
          type="text"
          value={username}
          onChange={e => setName(e.target.value)}/>

        </div>
      );
}

export default Camaraupload;
