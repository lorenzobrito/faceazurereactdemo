'use strict';
import React, { PureComponent ,Component  } from 'react';
import { AppRegistry, StyleSheet, Text, TouchableOpacity, View } from 'react-native';
import { RNCamera } from 'react-native-camera';

export default class Exampleapp extends Component  {
    constructor(props) {
        super(props);
        this.state = {
          takingPic: false,
          
        };
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
          flashMode={RNCamera.Constants.FlashMode.on}
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
          onGoogleVisionBarcodesDetected={({ barcodes }) => {
            console.log(barcodes);
          }}

        />
        <View style={{ flex: 0, flexDirection: 'row', justifyContent: 'center' }}>
          <TouchableOpacity onPress={this.takePicture.bind(this)} style={styles.capture}>
            <Text style={{ fontSize: 14 }}> SNAP </Text>
          </TouchableOpacity>
        </View>
      </View>
    );
  }
  // onFacesDetected = async (faces) => {

  //   if(faces.faces[0] && this.state.takingPic==false)
  //   {
  //       console.log('face detected')
  //       console.log(faces.faces[0].rollAngle);
  //       console.log(Math.abs(faces.faces[0].yawAngle));
  //       if( 1 > Math.abs(faces.faces[0].yawAngle) &&  Math.abs(faces.faces[0].yawAngle) > 0.1 )
  //       {
  //            this.setState({takingPic : true})
  //           console.log('center')
  //           const data = await this.camera.takePictureAsync({ base64: true });
  //           console.log('base64: ', data.base64);
  //         //  this.props.action.sendImageToServer(data.base64);
  //           var user = {
  //               dataUri : ","+data.base64,
  //               name : "identifyusers"
  //           };
  //       }
  //   }
  // }
  takePicture = async () => {
    
  };
}

const styles = StyleSheet.create({
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


