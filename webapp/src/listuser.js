import React, { useState,Fragment, useRef , useEffect} from 'react';
import ReactDOM from 'react-dom';
import './App.css';
import axios from 'axios';
function ListUser() {
  const apiUrl = "https://apifacelorbrito.azurewebsites.net/api/faceapi?code=LSPc9n6WBiC33f2u9GFlvrygmfLEVcw4OJgeDVaYMPJIBig6yKv6yA==";
  const [List, setList] = useState([]);
    
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
      
       {List.map((item,index)=>{
              return  <div key={index}>{item}</div>
       })}
        </div>
      );
}

export default ListUser;
