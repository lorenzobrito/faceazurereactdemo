import React, { Component ,PureComponent } from "react";

import {  StyleSheet, Text, TouchableOpacity, View,Alert  } from 'react-native';
import { RNCamera } from 'react-native-camera';
import axios from 'axios';
const apiUrl = "https://apifacelorbrito.azurewebsites.net/api/faceapi?code=LSPc9n6WBiC33f2u9GFlvrygmfLEVcw4OJgeDVaYMPJIBig6yKv6yA==";
const faceanalyze="https://apifacelorbrito.azurewebsites.net/api/faceanalyze?code=Q3923RB87m6wzE27ujCrunjk7F24ojapzKoS5DKGgEIHVzeY7OfI/g==";
class Facedetection extends PureComponent {
  constructor(props) {
    super(props);
    this.state = {
      titleText: "Bird's Nest",
      bodyText: "This is not really a bird nest.",
      takingPic : false
    };
  }

   analyzeUrl = (url) => {
    axios.post(faceanalyze, { 
      dataUri: url,
   name : '' })
     .then(res => {
       console.log(res);
       console.log(res.data);
       //setResult(res.data);
       Alert.alert(
        'Usuario Identificado',
        res.data[0],
        [
          {
            text: 'Ask me later',
            onPress: () => this.setState({takingPic : false})
          },
          {
            text: 'Cancel',
            onPress: () => console.log('Cancel Pressed'),
            style: 'cancel'
          },
          { text: 'OK', onPress: () => console.log('OK Pressed') }
        ],
        { cancelable: false }
      );
     }).catch(function (error) {
      console.log(error.response.statusText);
     
     // setResult(error.response.statusText);
 });
  }
  
     upload = (dataUri) => {
        axios.post(apiUrl, { 
           dataUri: dataUri,
        name : "identifyusers" })
          .then(res => {
            console.log(res);
            console.log(res.data.dataUri);
              this.analyzeUrl(res.data.dataUri);
          }).catch(function (error) {
            console.log(error.response.statusText);
           
           // setResult(error.response.statusText);
       });
      }
  takePicture = async () => {
      console.log('hello')
      const data = await this.camera.takePictureAsync({ base64: true });
      // console.log('base64: ', data.base64);
     //  this.props.action.sendImageToServer(data.base64);
       var user = {
           dataUri : ","+data.base64,
           name : "identifyusers"
       };
       this.upload(","+data.base64);
  }
  //= ({faces}) => {
    onFaceDetected = async (faces) => {
       if(faces.faces[0] && this.state.takingPic==false)
    {
        console.log('face detected')
        console.log(faces.faces[0].rollAngle);
        console.log(Math.abs(faces.faces[0].yawAngle));
        if( 1 > Math.abs(faces.faces[0].yawAngle) &&  Math.abs(faces.faces[0].yawAngle) > 0.1 )
        {
             this.setState({takingPic : true})
            console.log('center')
            const data = await this.camera.takePictureAsync({ base64: true });
           // console.log('base64: ', data.base64);
          //  this.props.action.sendImageToServer(data.base64);
            var user = {
                dataUri : ","+data.base64,
                name : "identifyusers"
            };
            this.upload(","+data.base64);
        }
    }
 
  }

  render() {
    return (
        <View style={styles.container}>
        <RNCamera
          ref={ref => {
            this.camera = ref;
          }}
          style={styles.preview}
          type={RNCamera.Constants.Type.front}
          
          androidCameraPermissionOptions={{
            title: 'Permission to use camera',
            message: 'We need your permission to use your camera',
            buttonPositive: 'Ok',
            buttonNegative: 'Cancel',
          }}
          androidRecordAudioPermissionOptions={{
            title: 'Permission to use audio recording',
            message: 'We need your permission to use your audio',
            buttonPositive: 'Ok',
            buttonNegative: 'Cancel',
          }}
         // onFacesDetected={this.onFaceDetected}
        

        />
        <View style={{ flex: 0, flexDirection: 'row', justifyContent: 'center' }}>
          <TouchableOpacity onPress={this.takePicture} style={styles.capture}>
            <Text style={{ fontSize: 14 }}> SNAP </Text>
          </TouchableOpacity>
        </View>
      </View>
    );
  }
}


const styles = StyleSheet.create({
    baseText: {
        fontFamily: "Cochin"
      },
      titleText: {
        fontSize: 20,
        fontWeight: "bold"
      },
    container: {
      flex: 1,
      flexDirection: 'column',
      backgroundColor: 'black',
    },
    preview: {
      flex: 1,
      justifyContent: 'flex-end',
      alignItems: 'center',
    },
    capture: {
      flex: 0,
      backgroundColor: '#fff',
      borderRadius: 5,
      padding: 15,
      paddingHorizontal: 20,
      alignSelf: 'center',
      margin: 20,
    },
  });
export default Facedetection;