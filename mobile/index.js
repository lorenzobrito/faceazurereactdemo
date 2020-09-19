/**
 * @format
 */

import {AppRegistry} from 'react-native';
import App from './App';
import {name as appName} from './app.json';
import Exampleapp from './View/Camaratest'
import Facedetection from './View/Facedetection'
AppRegistry.registerComponent(appName, () => Facedetection);
