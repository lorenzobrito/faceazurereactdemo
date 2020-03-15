import React, { useState,Fragment, useRef , useEffect} from 'react';
import ReactDOM from 'react-dom';
import './App.css';
import axios from 'axios';
function ListUser() {
  const apiUrl = "https://apifacelorbrito.azurewebsites.net/api/faceapi?code=LSPc9n6WBiC33f2u9GFlvrygmfLEVcw4OJgeDVaYMPJIBig6yKv6yA==";
  const [List, setList] = useState([]);
  const [Files, setFiles] = useState([]);
  const [Container, setContainer] = useState('');
  function getFiles (item) {
    axios.get(apiUrl+'&container='+item)
      .then(res => {
        console.log(res);
        console.log(res.data);
        setFiles(res.data)
      })
  }
      useEffect(() => {
        console.log('calling use');
        const fetchData = async () => {
          const result = await axios(apiUrl);
          setList(result.data);
          
        };
      
        fetchData();
      }, []);
     
      return (
        <div>
      <div>
       {List.map((item,index)=>{
              return  <div key={index}>
              <button onClick={() => getFiles(item)}>{item}</button>
              </div>
       })}
       </div>
       <div>
       {Files.map((item,index)=>{
              return  <div key={index}>
              <button >{item}</button>
              <img src={item} ></img>
              </div>
       })}
       </div>
        </div>
      );
}

export default ListUser;
