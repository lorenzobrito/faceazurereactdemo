import React, { useState } from 'react';
import logo from './logo.svg';
import './App.css';
import axios from 'axios';
function Facereconition() {
  const ApiUrl="https://apifacelorbrito.azurewebsites.net/api/faceanalyze?code=Q3923RB87m6wzE27ujCrunjk7F24ojapzKoS5DKGgEIHVzeY7OfI/g==";
  const [result, setResult] = useState('');
  const [url, setURl] = useState('');
  function analyzeUrl() {
    axios.post(ApiUrl, { 
      dataUri: url,
   name : '' })
     .then(res => {
       console.log(res);
       console.log(res.data);
       setResult(res.data);
     })
  }
  return (
    <div>
       Url to be analyze 
       {result}
       <input
          type="text"
          value={url}
          onChange={e => setURl(e.target.value)}/>
         
         <button onClick={() => analyzeUrl()}>Upload</button>
    </div>
  );
}

export default Facereconition;
